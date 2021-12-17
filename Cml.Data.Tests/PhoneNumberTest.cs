using Xunit;

namespace Cml.Data.Tests
{
	public class PhoneNumberTest
	{
		private PhoneNumber input = new PhoneNumber();

		[Theory]
		[InlineData("60", "123456789")]
		[InlineData("+60", "123456789")]
		public void ConstructorTest_CorrectCodeAndNo_FullContactNo(string contactCode, string contactNo)
		{
			var phone1 = new PhoneNumber(contactCode + contactNo);
			var phone2 = new PhoneNumber(contactNo, contactCode);

			Assert.Equal(phone1.FullNumber, phone2.FullNumber);
		}

		[Theory]
		[InlineData("60", "123456789")]
		[InlineData("+60", "123456789")]
		public void FullContactNo_CorrectCodeAndNo_FullContactNo(string contactCode, string contactNo)
		{
			test_FullContactNo(contactCode, contactNo, "+60123456789");
		}

		[Theory]
		[InlineData("60", "123456789")]
		[InlineData(" 60 ", "123456789 ")]
		[InlineData("60", "12 3456789")]
		[InlineData("6 0", "12 3456789	")]
		[InlineData("+60", "123456789")]
		[InlineData("+ 6 0", "123456789")]
		public void FullContactNo_WhiteSpacesInContactNo_FullContactNo(string contactCode, string contactNo)
		{
			test_FullContactNo(contactCode, contactNo, "+60123456789");
		}

		[Theory]
		[InlineData("+60", "123456789")]
		[InlineData("+60", "0123456789")]
		[InlineData("+60 ", "01 23456789 ")]
		[InlineData("+6 0 ", "01 23456789 ")]
		[InlineData("60", "0123456789")]
		[InlineData("60 ", " 0123456789 ")]
		[InlineData("6 0 ", "0123456789 ")]
		public void FullContactNo_60012ContactNo_FullContactNo(string contactCode, string contactNo)
		{
			test_FullContactNo(contactCode, contactNo, "+60123456789");
		}

		[Theory]
		[InlineData("+60", "623456789")]
		[InlineData("+60", "0623456789")]
		[InlineData("+6 0 ", "06 234-56789 ")]
		[InlineData("60", "0623456789")]
		[InlineData("6 0 ", "6-23456 789 ")]
		[InlineData("6 0 ", "06-23456789 ")]
		[InlineData("6 0 ", "600623456789 ")]
		public void FullContactNo_60062ContactNo_FullContactNo(string contactCode, string contactNo)
		{
			test_FullContactNo(contactCode, contactNo, "+60623456789");
		}

		[Theory]
		[InlineData(null, "60123456789")]
		[InlineData(null, "+60123456789")]
		[InlineData("", "60123456789")]
		[InlineData("", "6012-3456789")]
		[InlineData("  ", "+60123456789")]
		[InlineData("  ", "+6012-3456789")]
		//[ExpectedException(typeof(ArgumentException))]
		public void FullContactNo_EmptyContactCode_FullContactNo(string contactCode, string contactNo)
		{
			test_FullContactNo(contactCode, contactNo, "+60123456789");
		}

		private void test_FullContactNo(string contactCode, string contactNo, string expectedFullNumber)
		{
			input.CountryCode = contactCode;
			input.Value = contactNo;

			var fullContactNo = input.FullNumber;
			Assert.Equal(expectedFullNumber, fullContactNo);
		}

		[Theory]
		[InlineData("+60", "123456789")]
		[InlineData("+60", "0123456789")]
		[InlineData("+60 ", "01 23456789 ")]
		[InlineData("+6 0 ", "01 23456789 ")]
		[InlineData("60", "0123456789")]
		[InlineData("60 ", " 012-345 6789 ")]
		[InlineData("6 0 ", " 0123456789 ")]
		[InlineData("", " 60123456789 ")]
		[InlineData(null, " +60123456789 ")]
		[InlineData(null, " +600123456789 ")]
		[InlineData("+60", "+600123456789")]
		[InlineData("", " +600123456789 ")]
		public void FullContactNo_60012ContactNo_NumberWithoutCode(string contactCode, string contactNo)
		{
			input.CountryCode = contactCode;
			input.Value = contactNo;

			var numberOnly = input.NumberOnly;
			Assert.Equal("123456789", numberOnly);
		}

		[Theory]
		[InlineData(null, " +60623456789 ")]
		[InlineData(null, " +600623456789 ")]
		[InlineData(null, " +6060623456789 ")]
		[InlineData("", "+6060623456789")]
		[InlineData("+60", "+6060623456789")]
		public void FullContactNo_6060ddddContactNo_NumberWithoutCode(string contactCode, string contactNo)
		{
			input.CountryCode = contactCode;
			input.Value = contactNo;

			var numberOnly = input.NumberOnly;
			Assert.Equal("623456789", numberOnly);
		}

		[Theory]
		[InlineData("60", "0123456789")]
		[InlineData("60 ", " 012-345 6789 ")]
		[InlineData("6 0 ", " 0123456789 ")]
		public void PhoneNumber_Comparision_Equality(string code, string contactNo)
		{
			var phone1 = new PhoneNumber(code + contactNo);
			var phone2 = new PhoneNumber(contactNo, code);
			var phone3 = new PhoneNumber(contactNo, code);
			var phoneX = new PhoneNumber(contactNo + "69", code);

			Assert.Equal(phone1, phone1);
			Assert.True(phone1 == phone1);

			Assert.Equal(phone1, phone2);
			Assert.Equal(phone1, phone3);
			Assert.Equal(phone2, phone3);

			Assert.True(phone1 == phone2);
			Assert.True(phone1 == phone3);
			Assert.True(phone2 == phone3);

			//not equal
			Assert.NotEqual(phone1, phoneX);
			Assert.NotEqual(phone2, phoneX);
			Assert.NotEqual(phone3, phoneX);

			Assert.True(phone1 != phoneX);
			Assert.True(phone2 != phoneX);
			Assert.True(phone3 != phoneX);

			Assert.False(phone1 == phoneX);
			Assert.False(phone2 == phoneX);
			Assert.False(phone3 == phoneX);
		}

		[Theory]
		[InlineData(null, "389777988881")]
		[InlineData(null, "+389777988881")]
		[InlineData("", "389777988881")]
		[InlineData("389", "389777988881")]
		[InlineData("389", "+389777988881")]
		[InlineData("+389", "+389777988881")]
		[InlineData(" +389 ", " +3897 779 88881")]
		[InlineData("  ", "+389777988881")]
		[InlineData("  ", "+3897779-88881")]
		public void FullContactNo_389Code_FullContactNo(string contactCode, string contactNo)
		{
			test_FullContactNo(contactCode, contactNo, "+389777988881");
		}

		[Fact]
		public void Null()
		{
			PhoneNumber phoneNumber = null;

			Assert.True(phoneNumber == null);
			Assert.True(phoneNumber is null);
			Assert.Equal(null, phoneNumber);
		}

		[Fact]
		public void NotNull()
		{
			PhoneNumber phoneNumber = new PhoneNumber();

			Assert.NotNull(phoneNumber);
			Assert.False(phoneNumber == null);
			Assert.True(phoneNumber != null);
			Assert.False(phoneNumber is null);
		}
	}
}
