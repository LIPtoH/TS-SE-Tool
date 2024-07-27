/*
   Copyright 2016-2022 LIPtoH <liptoh.codebase@gmail.com>

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace TS_SE_Tool.Utilities {
    public static class TextExtensions {
        public static List<Version> ParseVersions(this string input) {
            var inputs = Regex.Split(input, ",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
            var versions = new List<Version>();
            foreach (var item in inputs) {
                if (item.Contains("-")) {
                    var rangeParts = item.Split('-');
                    if (rangeParts.Length == 2) {
                        if (Version.TryParse(rangeParts[0], out var startVersion) && Version.TryParse(rangeParts[1], out var endVersion)) {
                            for (var currentVersion = startVersion; currentVersion <= endVersion; currentVersion = new Version(currentVersion.Major, currentVersion.Minor + 1)) {
                                versions.Add(currentVersion);
                            }
                        }
                    }
                } else {
                    if (Version.TryParse(item, out var singleVersion)) {
                        versions.Add(singleVersion);
                    }
                }
            }
            return versions;
        }
        public static string Format(this string format, params object[] args) {
            return string.Format(format.Remove("\\r").Replace("\\n", Environment.NewLine), args);
        }
        public static string TrimTrailingZeros(this string input) {
            var parts = input.Split('.');
            for (int i = 0; i < parts.Length - 1; i++) {
                parts[i] = parts[i].TrimEnd('0');
                if (parts[i] == "0") parts[i] = "1"; // Check if the part became "0" due to trimming // Keep at least one zero
            }
            return string.Join(".", parts);
        }
        #region Json
        public class DirectoryInfoConverter : JsonConverter<DirectoryInfo> {
            public override DirectoryInfo Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
                return new DirectoryInfo(reader.GetString());
            }
            public override void Write(Utf8JsonWriter writer, DirectoryInfo value, JsonSerializerOptions options) {
                writer.WriteStringValue(value.FullName);
            }
        }
        public class FileInfoConverter : JsonConverter<FileInfo> {
            public override FileInfo Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
                return new FileInfo(reader.GetString());
            }
            public override void Write(Utf8JsonWriter writer, FileInfo value, JsonSerializerOptions options) {
                writer.WriteStringValue(value.FullName);
            }
        }
        //public static JsonSerializerOptions JsonSerializerOptions = new() { }
        /// <summary>
        /// Serializes the given object to a JSON string with optional indentation.
        /// </summary>
        /// <param name="input">The object to serialize.</param>
        /// <param name="indent">Indicates whether to apply indentation to the output JSON.</param>
        /// <returns>A JSON string representation of the object.</returns>
        public static string ToJson(this object input, bool indent = false) {
            //try {
            var options = new JsonSerializerOptions() {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                //PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None,
                WriteIndented = indent
            };
            options.Converters.Add(new FileInfoConverter()); options.Converters.Add(new DirectoryInfoConverter());
            return JsonSerializer.Serialize(input, options);
            //} catch (Exception ex) {
            //    IO_Utilities.ErrorLogWriter($"Error serializing object: {ex.Message}");
            //    return input.ToString();
            //}
        }
        #endregion Json

        public static bool ToFile(this string text, string outputFile, bool overwrite = false) => ToFile(text, new FileInfo(outputFile), overwrite);
        public static bool ToFile(this string text, FileInfo outputFile, bool overwrite = false) {
            try {
                if (outputFile.Exists && !overwrite) {
                    throw new IOException($"Not overwriting existing file: {outputFile.FullString()}");
                }
                using (StreamWriter writer = new StreamWriter(outputFile.FullName)) {
                    writer.Write(text);
                }
                return true;
            } catch (IOException ex) {
                IO_Utilities.ErrorLogWriter(ex.Message);
                return false;
            }
        }

        public static string Join(this IEnumerable<object> inputs, string seperator = ", ") {
            return string.Join(seperator, inputs.Select(i => i.ToString()));
        }
        /// <summary>
        /// Returns a new string in which all occurrences of a specified string in the current instance are replaced with another specified string.
        /// </summary>
        /// <param name="str">The string performing the replace method.</param>
        /// <param name="oldValue">The string to be replaced.</param>
        /// <param name="newValue">The string replace all occurrances of oldValue.</param>
        /// <param name="comparisonType">Type of the comparison.</param>
        /// <returns></returns>
        public static string Replace(this string str, string oldValue, string @newValue, StringComparison comparisonType = StringComparison.CurrentCulture) {
            @newValue = @newValue ?? string.Empty;
            if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(oldValue) || oldValue.Equals(@newValue, comparisonType)) {
                return str;
            }
            int foundAt;
            while ((foundAt = str.IndexOf(oldValue, 0, comparisonType)) != -1) {
                str = str.Remove(foundAt, oldValue.Length).Insert(foundAt, @newValue);
            }
            return str;
        }
        public static string Remove(this string _input, string text, StringComparison comparisonType = StringComparison.CurrentCulture) => _input.Replace(text, string.Empty, comparisonType);
        public static string RemoveAll(this string _input, params string[] texts) {
            foreach (var text in texts) {
                _input = _input.Remove(text, StringComparison.OrdinalIgnoreCase);
            }
            return _input;
        }
        public static string ReplaceLastOccurrence(this string Source, string Find, string Replace) {
            int place = Source.LastIndexOf(Find);
            if (place == -1) {
                return Source;
            }

            string result = Source.Remove(place, Find.Length).Insert(place, Replace);
            return result;
        }
        public static string Ext(this string text, string extension) {
            return text + "." + extension;
        }
        public static string Quote(this string text) {
            return SurroundWith(text, "\"");
        }
        public static string Enclose(this string text) {
            return SurroundWith(text, "(", ")");
        }
        public static string Brackets(this string text) {
            return SurroundWith(text, "[", "]");
        }
        public static string SurroundWith(this string text, string surrounds) {
            return surrounds + text + surrounds;
        }
        public static string SurroundWith(this string text, string starts, string ends) {
            return starts + text + ends;
        }
    }

    public class TextUtilities {
        public static string FromHexToString(string _input) {
            try {
                byte[] raw = new byte[_input.Length / 2];
                for (int i = 0; i < raw.Length; i++) {
                    raw[i] = Convert.ToByte(_input.Substring(i * 2, 2), 16);
                }

                return Encoding.UTF8.GetString(raw); //UTF8
            } catch {
                return null;
            }
        }

        public static string FromUtfHexToString(string _input) {
            try {
                string result = "";

                for (int i = 0; i < _input.Length; i++) {
                    if (i < _input.Length - 2 && _input.Substring(i, 2) == "\\x") {
                        string temp = "";
                    nextHD:

                        temp += _input.Substring(i + 2, 2);
                        i = i + 4;

                        if (i < _input.Length - 2 && _input.Substring(i, 2) == "\\x")
                            goto nextHD;
                        else {
                            result += FromHexToString(temp);
                            i--;
                        }
                    } else
                        result += _input[i];
                }

                return result;
            } catch {
                return null;
            }
        }

        public static string FromStringToHex(string _input) {
            try {
                return ByteArrayToString(Encoding.UTF8.GetBytes(_input.ToCharArray()));
            } catch {
                return null;
            }
        }

        public static string FromStringToOutputString(string _input) {
            try {
                if (_input == "" || _input == null)
                    return "\"\"";          //Empty

                string byteArrayString = StringToByteArrayStringFull(_input);

                if (CheckStringAlphaNumeric(byteArrayString))
                    return _input;                          //Simple AlphaNumeric string 
                else
                    return "\"" + byteArrayString + "\"";   //Else
            } catch {
                return null;
            }
        }

        public static string FromOutputStringToString(string _input) {
            try {
                string byteArrayString = StringToByteArrayStringFull(_input);

                if (!CheckStringAlphaNumeric(byteArrayString))
                    return _input.Substring(1, _input.Length - 2);
                else
                    return _input;
            } catch {
                return null;
            }
        }

        public static string ByteArrayToString(byte[] _ba) {
            return BitConverter.ToString(_ba).Replace("-", "");
        }

        public static string StringToByteArrayStringFull(string _input) {
            string output = "";

            foreach (char x in _input) {
                byte[] testBytes = Encoding.UTF8.GetBytes(new char[] { x });

                if (testBytes.Length == 1) {
                    output += x;
                } else {
                    foreach (byte xByte in testBytes)
                        output += "\\x" + xByte.ToString("x");
                }
            }

            return output;
        }

        internal static string CapitalizeWord(string _input) {
            // Check for empty string.
            if (string.IsNullOrEmpty(_input)) {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(_input[0]) + _input.Substring(1).ToLower();
        }

        public static bool CheckStringAlphaNumeric(string _input) {
            foreach (char xChar in _input) {
                if (!char.IsLetterOrDigit(xChar) && !(xChar == '_'))
                    return false;
            }

            return true;
        }

        private bool checkStringContainsUnescape(string _input) {
            if (_input.Contains(' ') || _input != System.Text.RegularExpressions.Regex.Unescape(_input))
                return true;
            else
                return false;
        }

        internal static string CheckAndClearStringFromQuotes(string _input) {
            string processingResult = "";

            if (_input.StartsWith("\"") && _input.EndsWith("\"")) {
                string innerData = _input.Substring(1, _input.Length - 2);//.Remove(_input.Length - 1, 1).Remove(0, 1);

                if (innerData == "")
                    return "";

                processingResult = TextUtilities.FromUtfHexToString(innerData);
            }

            return (processingResult == "") ? _input : processingResult;
        }

        private static readonly Regex regexDigit = new Regex(@"\d+");
        internal static bool ExtractFirstNumber(string _text, out int result) {
            result = -1;

            var match = regexDigit.Match(_text);

            if (match.Success)
                result = Convert.ToInt32(match.Value);

            return match.Success;
        }
    }
}
