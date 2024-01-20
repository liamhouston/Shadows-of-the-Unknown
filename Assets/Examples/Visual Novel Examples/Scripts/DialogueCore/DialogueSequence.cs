using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Narrative
{
    /// <summary>
    /// A data container for dialogues. 
    /// </summary>
    /// <seealso cref="DialogueCSVParser"/>
    public class DialogueSequence : IEnumerable
    {
        /// Line data
        private List<List<string>> lines;

        /// <summary>
        /// Constructor that duplicates the contents of the source data (prevents overwriting)
        /// </summary>
        /// <param name="srcLines">Source line data</param>
        public DialogueSequence(List<List<string>> srcLines)
        {
            //Create list as duplicate
            lines = new List<List<string>>(srcLines);
            for (int i = 0; i < lines.Count; i++)
            {
                lines[i] = new List<string>(lines[i]);
            }
        }

        /// <summary>
        /// Constructor from a TextAsset. Doesn't need duplication since the parser returns a new list.
        /// </summary>
        /// <param name="sourceAsset">Source asset, the CSV file directly usually</param>
        public DialogueSequence(TextAsset sourceAsset)
        {
            //Get data from parser
            lines = DialogueCSVParser.ReadCSVLines(sourceAsset.text);
            lines = DialogueCSVParser.CleanCSVInput(lines);
        }

        /// <summary>
        /// Quick check that the line number exists
        /// </summary>
        /// <param name="lineNum">Line index to check</param>
        /// <returns></returns>
        public bool HasLine(int lineNum)
        {
            return lines.Count > lineNum;
        }

        /// <summary>
        /// Get a duplicate of the row in the file. If the source had any empty lines removed this will not be the same as the line number in the CSV itself.
        /// </summary>
        /// <param name="lineNum">Line index</param>
        /// <returns>A list of the cells in the line</returns>
        public List<string> GetRow(int lineNum)
        {
            return new List<string>(lines[lineNum]);
        }

        /// <summary>
        /// Obtains a subsequence of the lines in this DialogueSequence and returns them as a new DialogueSequence.
        /// </summary>
        /// <param name="start">First line in new subsequence</param>
        /// <param name="end">Last line in new subsequence, inclusive</param>
        /// <returns>A DialogueSequence object containing the subsequence</returns>
        public DialogueSequence GetSubSequenceObj(int start, int end)
        {
            return new DialogueSequence(GetSubSequence(start, end));
        }
        /// <summary>
        /// Obtains a subsequence of the lines in this DialogueSequence and returns them as a List of Lists.
        /// </summary>
        /// <param name="start">First line in new subsequence</param>
        /// <param name="end">Last line in new subsequence, inclusive</param>
        /// <returns>A duplicate list of lists containing the subsequence</returns>
        public List<List<string>> GetSubSequence(int start, int end)
        {
            List<List<string>> subsequence = new List<List<string>>();
            for (int i = start; i <= end; i++)
            {
                subsequence.Add(GetRow(i));
            }
            return subsequence;
        }

        /// <summary>
        /// Gets the value in the given row and column of the CSV data.
        /// Doesn't error check for validity.
        /// </summary>
        /// <returns>Contents of the cell.</returns>
        public string GetCell(int lineNum, int columnNum)
        {
            return (lines[lineNum][columnNum]);
        }

        /// <summary>
        /// Gets the dialogue from the line. Currently defined as the last non-tag non-empty entry in the row.
        /// </summary>
        public string GetRowDialogue(int lineNum)
        {
            List<string> row = lines[lineNum];
            //Find last valid entry
            for (int i = row.Count - 1; i > 0; i--)
            {
                if (row[i] != "")//todo: also check tags if any
                {
                    return row[i];
                }
            }
            return "";//There was no valid value so return empty
        }

        /// <summary>
        /// Returns the character name for this line if it exists, otherwise returns empty string.
        /// </summary>
        /// <param name="lineNum">Line number</param>
        public string GetRowName(int lineNum)
        {
            List<string> row = lines[lineNum];

            if (row.Count>0){
                return row[0];
            }

            return "";//No valid entry so return empty
        }
        
        /// <summary>
        /// Gets the portrait id for the left portrait if it exists
        /// </summary>
        /// <param name="lineNum">Line number</param>
        public string GetRowPortraitLeft(int lineNum)
        {
            List<string> row = lines[lineNum];

            //Find a valid entry
            if(row.Count>1){
                if (DialoguePortraitContainer.IsValidPortraitName(row[1]))
                {
                    return row[1];
                }
            }

            return "";//No valid entry so return empty
        }

        /// <summary>
        /// Gets the portrait id for the right portrait if it exists
        /// </summary>
        /// <param name="lineNum">Line number</param>
        public string GetRowPortraitRight(int lineNum)
        {
            List<string> row = lines[lineNum];

            //Find a valid entry
            if(row.Count>2){
                if (DialoguePortraitContainer.IsValidPortraitName(row[2]))
                {
                    return row[2];
                }
            }

            return "";//No valid entry so return empty
        }

        /// <summary>
        /// Gets the sound id for the right sound if it exists
        /// </summary>
        /// <param name="lineNum">Line number</param>
        public string GetRowSoundClip(int lineNum)
        {
            List<string> row = lines[lineNum];

            //Find a valid entry
            if(row.Count>3){
                if (DialogueSounds.IsValidSoundName(row[3]))
                {
                    return row[3];
                }
            }

            return "";//No valid entry so return empty
        }

        /// <summary>
        /// Checks if the sequence is empty, defined as having zero lines.
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return lines.Count == 0;
        }

        /// <summary>
        /// Allows enumeration over this object (eg in foreach loop).
        /// </summary>
        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)lines).GetEnumerator();
        }
    }
}