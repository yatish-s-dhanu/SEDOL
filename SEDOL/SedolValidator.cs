using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEDOL
{
    public class SedolValidator : ISedolValidator
    {
        private readonly int[] iWeightFactor = { 1, 3, 1, 7, 3, 9, 1 };
        public ISedolValidationResult ValidateSedol(string input)
        {
            SedolValidationResult result = null;
            int len = string.IsNullOrEmpty(input) ? 0 : input.Length;
            string regEx = @"[\W_-[\s]]";
            int checkSum = 0, chkDigit = 0;

            if (len == 0 || len !=7 )
            {
                result = new SedolValidationResult
                {
                    InputString = input,
                    IsValidSedol = false,
                    IsUserDefined = false,
                    ValidationDetails = "Input string was not 7-characters long"
                };
            }

            else 
            {
                //invalid SEDOL
                if (System.Text.RegularExpressions.Regex.IsMatch(input, regEx))
                {
                    result = new SedolValidationResult
                    {
                        InputString = input,
                        IsValidSedol = false,
                        IsUserDefined = false,
                        ValidationDetails = "SEDOL contains invalid characters"
                    };
                }
                else
                {
                    //Calculate checksum
                    for (int i = 0; i < 6; i++)
                    {
                        if (Char.IsDigit(input[i]))
                            checkSum += ((((int)input[i] - 48) * iWeightFactor[i]));

                        else if (Char.IsLetter(input[i]))
                            checkSum += (((int)Char.ToUpper(input[i]) - 55) * iWeightFactor[i]);
                    }

                    chkDigit = (10 - (checkSum % 10)) % 10;


                    if (Char.IsDigit(input[6]))
                    {
                        //Valid non user define SEDOL
                        if (chkDigit == ((int)input[6] - 48))
                        {
                            result = new SedolValidationResult
                            {
                                InputString = input,
                                IsValidSedol = true,
                                IsUserDefined = false,
                                ValidationDetails = null
                            };
                        }
                        else
                        {
                            //Checksum digit does not agree with the rest of the input
                            result = new SedolValidationResult
                            {
                                InputString = input,
                                IsValidSedol = false,
                                IsUserDefined = false,
                                ValidationDetails = "Checksum digit does not agree with the rest of the input"
                            };
                        }

                    }
                }
                
            }
            
            
            return result;
        }
    }

    public class SedolValidationResult : ISedolValidationResult
    {
        private string inputString;
        private bool isValidSedol;
        private bool isUserDefined;
        private string validationDetails;

        public string InputString {
            get {
                return this.inputString;
            }
            set {
                this.inputString = value;
            }
        }

        public bool IsValidSedol {
            get
            {
                return this.isValidSedol;
            }
            set
            {
                this.isValidSedol = value;
            }
        }

        public bool IsUserDefined
        {
            get
            {
                return this.isUserDefined;
            }
            set
            {
                this.isUserDefined = value;
            }
        }

        public string ValidationDetails
        {
            get
            {
                return this.validationDetails;
            }
            set
            {
                this.validationDetails = value;
            }
        }
    }
}
