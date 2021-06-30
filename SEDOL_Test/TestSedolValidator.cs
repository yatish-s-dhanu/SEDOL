using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SEDOL;

namespace SEDOL_Test
{
    [TestClass]
    public class TestSedolValidator
    {

        void CompareResult(SedolValidationResult expected, ISedolValidationResult actual)
        {
            Assert.AreEqual(expected.IsValidSedol, actual.IsValidSedol);
            Assert.AreEqual(expected.IsUserDefined, actual.IsUserDefined);
            Assert.AreEqual(expected.ValidationDetails, actual.ValidationDetails);
        }

        [TestMethod]
        public void Test_InputLength()
        {
            string input = null;

            var expected = new SedolValidationResult();
            expected.InputString = input;
            expected.IsValidSedol = false;
            expected.IsUserDefined = false;
            expected.ValidationDetails = "Input string was not 7-characters long";

            var test = new SedolValidator();

            //null
            ISedolValidationResult actual = test.ValidateSedol(null);
            CompareResult(expected, actual);

            //Empty string
            input = "";
            actual = test.ValidateSedol(input);
            CompareResult(expected, actual);


            //Input.length < 7
            input = "12";
            actual = test.ValidateSedol(input);
            CompareResult(expected, actual);


            //Input.length > 7
            input = "123456789";
            actual = test.ValidateSedol(input);
            CompareResult(expected, actual);


        }

        [TestMethod]
        public void Test_InvalidChecksum()
        {
            string input = "1234567";
            var expected = new SedolValidationResult();
            expected.InputString = input;
            expected.IsValidSedol = false;
            expected.IsUserDefined = false;
            expected.ValidationDetails = "Checksum digit does not agree with the rest of the input";

            var test = new SedolValidator();
            var actual = test.ValidateSedol(input);
            CompareResult(expected, actual);
        }

        [TestMethod]
        public void Test_ValidNonUserSEDOL()
        {
            string input = "0709954";
            var expected = new SedolValidationResult();
            expected.InputString = input;
            expected.IsValidSedol = true;
            expected.IsUserDefined = false;
            expected.ValidationDetails = null;

            var test = new SedolValidator();
            var actual = test.ValidateSedol(input);
            CompareResult(expected, actual);

            input = "B0YBKJ7";
            actual = test.ValidateSedol(input);
            CompareResult(expected, actual);
        }

        [TestMethod]
        public void Test_InvalidCharacter()
        {
            string input = "9123_51";
            var expected = new SedolValidationResult();
            expected.InputString = input;
            expected.IsValidSedol = false;
            expected.IsUserDefined = false;
            expected.ValidationDetails = "SEDOL contains invalid characters";

            var test = new SedolValidator();
            var actual = test.ValidateSedol(input);
            CompareResult(expected, actual);

            input = "VA.CDE8";
            actual = test.ValidateSedol(input);
            CompareResult(expected, actual);
        }
    }
}
