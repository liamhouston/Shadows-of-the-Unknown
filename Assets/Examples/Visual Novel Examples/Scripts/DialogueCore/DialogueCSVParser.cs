using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Narrative
{
    /// <summary>
    /// A utility class that parses text input from CSV (comma seperated values) for dialogue.
    /// </summary>
    public class DialogueCSVParser
    {

        /// <summary>
        /// Reads a list of data from a csv file
        /// Note: this function assumes a valid file format and will likely break if given invalid data
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static List<List<string>> ReadCSVLines(string text)
        {
            const char delimiter = ','; //seperates cells

            //Declare data
            List<List<string>> fileLines = new List<List<string>>();
            List<string> currentLine = new List<string>();

            //Cleans newlines that may be different by OS
            text = text.Replace("\r\n", "\n");


            int cursorL = 0; //current reading position

            //Loop through entire input data
            while (cursorL < text.Length)
            {
                //string cursorText = text.Substring(cursorL);//makes the current text easier to see in debug mode

                bool isQuoted = (text[cursorL] == '"');
                if (!isQuoted)
                {
                    //Detect empty cells
                    if (text[cursorL] == delimiter)
                    {
                        //New empty cell
                        currentLine.Add("");
                        cursorL++;
                        continue;
                    }

                    //Find delimiters and newlines
                    int nextDelim = text.IndexOf(delimiter, cursorL);
                    int nextLineBreak = text.IndexOf('\n', cursorL);
                    //If not found, replace -1 with end of string
                    if (nextDelim < 0) { nextDelim += text.Length; }
                    if (nextLineBreak < 0) { nextLineBreak += text.Length; }

                    //Find cell boundary
                    int cellLen = Mathf.Min(nextDelim, nextLineBreak) - cursorL;

                    //Record cell
                    string cell = text.Substring(cursorL, cellLen).Trim();
                    currentLine.Add(cell);

                    //Advance to next
                    cursorL += cellLen;

                    //Check delimiters
                    if (text[cursorL] == '\n')
                    {
                        //End of line, add to lines list
                        fileLines.Add(currentLine);
                        currentLine = new List<string>();
                        cursorL++;
                    }
                    else if (text[cursorL] == delimiter)
                    {
                        //Increment past delimiter
                        cursorL++;
                    }
                }
                else if (isQuoted)
                {
                    //Skip any inline quotes, denoted by two "
                    var cursorR = cursorL + 1;
                    //Move forward in input until we reach a quote or error
                    while (cursorR < text.Length)
                    {
                        char c = text[cursorR];
                        if (c == '"')
                        {
                            if (cursorR >= text.Length - 1)//end of file
                            {
                                //Add the cell, but this shouldn't typically happen so throw warning
                                Debug.LogWarning("DialogueCSVParser: End of file reached in inline string, CSV file may be incorrectly formatted.");
                                string cell = text.Substring(cursorL + 1);
                                currentLine.Add(cell);
                                cursorL = cursorR + 1;
                                break;
                            }
                            else if (text[cursorR + 1] == '"')//double "
                            {
                                cursorR += 2;//skip ahead
                                continue;
                            }
                            else
                            {
                                //Found a single quote
                                string cell = text.Substring(cursorL + 1, cursorR - cursorL - 1);
                                cell = cell.Replace("\"\"", "\"");//Replace doubles with singles
                                currentLine.Add(cell);
                                cursorL = cursorR + 2;
                                char nextch = text[cursorR + 1];
                                if (nextch == '\n')
                                {
                                    //End of row
                                    fileLines.Add(currentLine);
                                    currentLine = new List<string>();
                                }
                                break;
                            }
                        }
                        cursorR++;
                    }
                }
            }
            //Append any remaining cells
            if (currentLine.Count > 0 && fileLines.LastOrDefault() != currentLine)
            {
                fileLines.Add(currentLine);
            }

            return fileLines;
        }

        /// <summary>
        /// Cleans input data by removing blank cells and lines.
        /// </summary>
        /// <param name="sourceData"></param>
        /// <returns></returns>
        public static List<List<string>> CleanCSVInput(List<List<string>> sourceData)
        {
            //Remove empty cells
            foreach (List<string> line in sourceData)
            {
                line.RemoveAll(cell => string.IsNullOrEmpty(cell));
            }

            //Remove empty lines
            sourceData.RemoveAll(line => line.Count == 0);

            return sourceData;
        }
    }
}