using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.Objects.Handlers
{
    public static class TypeToStringHandler
    {
        static FieldInfo[] _keyFields;
        static List<object> _keyValues;

        static TypeToStringHandler()
        {
            _keyFields = typeof(Key).GetFields();
            _keyValues = _keyFields.Select(field => field.GetValue(typeof(Key))).Where(value => value != null).ToList(); //the Where null check is redundant but ehh... just in case
        }

        [Flags]
        public enum AllowedTypes
        {
            None = 0,
            Alphabet = 1,
            Numbers = 2,
            Symbols = 4,
            Period = 8,
            All = 16
        };

        static string[] _shiftKeyValues = new string[14] { ")", "!", "@", "#", "$", "%", "^", "&", "*", "(", "-", "=", "_", "+" };
        public static string GetOutputString(string inputString, AllowedTypes types)
        {

            int localCounter = 0;
            string keyString = inputString;
            foreach (object key in _keyValues)
            {
                if (key?.GetType() == typeof(int)) //Redundant, but here for good meassure
                {
                    if (types.HasFlag(AllowedTypes.Alphabet) || types.HasFlag(AllowedTypes.All))
                    {
                        if (Input.GetKeyDown((int)key) && (int)key >= 65 && (int)key <= 90)
                        {

                            if (Input.GetKey(Key.LEFT_SHIFT) || Input.GetKey(Key.RIGHT_SHIFT))
                            {
                                keyString += _keyFields[localCounter].Name.ToString().ToUpper();
                            }
                            else
                            {
                                keyString += _keyFields[localCounter].Name.ToString().ToLower();
                            }

                        }
                    }
                    if (types.HasFlag(AllowedTypes.Numbers) || types.HasFlag(AllowedTypes.All))
                    {
                        if (Input.GetKeyDown((int)key) && (int)key >= 48 && (int)key <= 57)
                        {
                            if (Input.GetKey(Key.LEFT_SHIFT) || Input.GetKey(Key.RIGHT_SHIFT))
                            {

                                if (types.HasFlag(AllowedTypes.Symbols) || types.HasFlag(AllowedTypes.All))
                                {
                                    keyString += _shiftKeyValues[((int)key - 48)];
                                }
                                else
                                {
                                    keyString += ((int)key - 48).ToString();
                                }

                            }
                            else
                            {
                                keyString += ((int)key - 48).ToString();
                            }
                        }
                        if (Input.GetKeyDown(61) && (int)key == 61)
                        {
                            if (Input.GetKey(Key.LEFT_SHIFT) || Input.GetKey(Key.RIGHT_SHIFT))
                            {
                                if (types.HasFlag(AllowedTypes.Symbols) || types.HasFlag(AllowedTypes.All))
                                {
                                    keyString += "+";
                                }
                                else
                                {
                                    keyString += "=";
                                }
                            }
                            else
                            {
                                keyString += "=";
                            }
                        }
                        if (Input.GetKeyDown(45) && (int)key == 45)
                        {
                            if (Input.GetKey(Key.LEFT_SHIFT) || Input.GetKey(Key.RIGHT_SHIFT))
                            {
                                if (types.HasFlag(AllowedTypes.Symbols) || types.HasFlag(AllowedTypes.All))
                                {
                                    keyString += "_";
                                }
                                else
                                {
                                    keyString += "-";
                                }

                            }
                            else
                            {
                                keyString += "-";
                            }
                        }
                    }
                }
                localCounter++;
            }
            if (types.HasFlag(AllowedTypes.Period) || types.HasFlag(AllowedTypes.All))
            {
                if (Input.GetKeyDown(Key.DOT))
                {
                    keyString += ".";
                }
                if (Input.GetKeyDown(Key.COMMA))
                {
                    keyString += ",";
                }
            }

            if (Input.GetKeyDown(Key.BACKSPACE))
            {
                if (keyString.Length > 0)
                    keyString = keyString.Substring(0, keyString.Length - 1);
            }
            if (Input.GetKeyDown(Key.SPACE) && !types.HasFlag(AllowedTypes.Period))
            {
                keyString += " ";
            }

            return keyString;
        }

    }
}
