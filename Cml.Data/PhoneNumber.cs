using System;
using System.Collections.Generic;
using System.Linq;

namespace Cml.Data
{
	public class PhoneNumber : IEquatable<PhoneNumber>
	{
		private string _inputPhoneNumber; //this may or may not be full phone number
		private string _countryCode; //this might contain country code again that already in inputPhoneNumber

		public PhoneNumber() { }

		public PhoneNumber(string value)
		{
			this.Value = value;
		}

		public PhoneNumber(string value, string countryCode)
		{
			this.Value = value;
			this.CountryCode = countryCode;
		}

		public string CountryCode
		{
			get
			{
				// return an existing
				if (_countryCode != null)
					return _countryCode;

				// else, extract from contactNo
				if (_inputPhoneNumber != null)
				{
					foreach (var entry in Country_Code_All)
					{
						var code = entry.Value;
						if (_inputPhoneNumber.StartsWith(code))
							return code;
					}

					// for Malaysia Code, case like 03dddddddd or 012ddddddd
					if (IsMalaysiaNumber(_inputPhoneNumber))
						return Malaysia_Code;
				}

				return null;
			}
			set
			{
				if (string.IsNullOrWhiteSpace(value))
					return;

				//remove all non digit
				_countryCode = new string(value.Where(char.IsDigit).ToArray());

				// for easier computation,
				// the starting plus + will not be consider as part of Country Code value
			}
		}

		public string Value
		{
			get { return NumberOnly; }
			set
			{
				if (string.IsNullOrWhiteSpace(value))
					throw new ArgumentException("phone number cannot be empty");

				//remove all non digit
				var contactNo = new string(value.Where(char.IsDigit).ToArray());

				if (contactNo == string.Empty)
					throw new ArgumentException($"invalid phone number {value}");

				this._inputPhoneNumber = contactNo;
			}
		}

		public string NumberOnly
		{
			get
			{
				var numberOnly = GetNumberOnly();

				//remove 6012ddd or 012ddd to 12ddd
				IsMalaysiaNumber(numberOnly, out int codeLength);

				return numberOnly.Substring(codeLength);
			}
		}

		private string GetNumberOnly()
		{
			var value = _inputPhoneNumber;
			int codeLength;

			if (_countryCode != null)
			{
				if (value.StartsWith(_countryCode))
					return value.Substring(_countryCode.Length);

				if (IsMalaysiaNumber(value, out codeLength))
					return value.Substring(codeLength);

				return value;
			}

			foreach (var entry in Country_Code_All)
			{
				var code = entry.Value;
				if (value.StartsWith(code))
					return value.Substring(code.Length);
			}

			if (IsMalaysiaNumber(value, out codeLength))
				return value.Substring(codeLength);

			return value;
		}

		public string FullNumber
		{
			get
			{
				var code = CountryCode;
				var value = _inputPhoneNumber;

				if (code == null)
					return value;

				if (IsMalaysiaNumber(value))
					return "+" + Malaysia_Code + NumberOnly;
				// e.g: 012ddddddd become +6012ddddddd

				return "+" + code + NumberOnly;
			}
		}

		public static bool IsMalaysiaNumber(string number)
		{
			return IsMalaysiaNumber(number, out var codeLength);
		}

		private static bool IsMalaysiaNumber(string number, out int codeLength)
		{
			// for Malaysia Code,
			// include case like 03dddddddd or 012ddddddd without 60
			// assume is Malaysia Code if match
			if (number.Length >= Malaysia_Number_Min_Length)
			{
				if (number.StartsWith("0")) //detect if Malaysia code is 0ddd or 60ddd
					codeLength = 1;
				else if (number.StartsWith(Malaysia_Code))
					codeLength = Malaysia_Code.Length;
				else
					codeLength = 0;
			}
			else
			{
				codeLength = 0;
			}

			return codeLength >= 1; // length of code, either it is 60-ddd or 0-ddd; length 2 or 1
		}

		public override bool Equals(object other)
		{
			if (other is null)
				return false;

			if (object.ReferenceEquals(this, other))
				return true;

			if (other.GetType() != this.GetType())
				return false;

			var other1 = (PhoneNumber)other;

			return this.Equals(other1);
		}

		public bool Equals(PhoneNumber other)
		{
			if (other is null)
				return false;

			return other.FullNumber == this.FullNumber;
		}

		public override int GetHashCode()
		{
			return this.FullNumber.GetHashCode();
		}

		public static bool operator ==(PhoneNumber phone1, PhoneNumber phone2)
		{
			if (phone1 is null)
			{
				if (phone2 is null)
					return true;
				else
					return false;
			}
			else
			{
				if (phone2 is null)
					return false;
				else
					return phone1.Equals(phone2);
			}
		}

		public static bool operator !=(PhoneNumber phone1, PhoneNumber phone2)
		{
			return !(phone1 == phone2);
		}

		private static readonly int Malaysia_Number_Min_Length = 10;
		public static readonly string Malaysia_Code = "60";

		public static readonly Dictionary<string, string> Country_Code_All = new Dictionary<string, string>()
		{
			{"Malaysia", Malaysia_Code},
			{"Abkhazia", "7840"},
			{"Afghanistan", "93"},
			{"Albania", "355"},
			{"Algeria", "213"},
			{"American Samoa", "1684"},
			{"Andorra", "376"},
			{"Angola", "244"},
			{"Anguilla", "1264"},
			{"Antigua and Barbuda", "1268"},
			{"Argentina", "54"},
			{"Armenia", "374"},
			{"Aruba", "297"},
			{"Ascension", "247"},
			{"Australia", "61"},
			{"Australian External Territories", "672"},
			{"Austria", "43"},
			{"Azerbaijan", "994"},
			{"Bahamas", "1242"},
			{"Bahrain", "973"},
			{"Bangladesh", "880"},
			{"Barbados", "1246"},
			{"Barbuda", "1268"},
			{"Belarus", "375"},
			{"Belgium", "32"},
			{"Belize", "501"},
			{"Benin", "229"},
			{"Bermuda", "1441"},
			{"Bhutan", "975"},
			{"Bolivia", "591"},
			{"Bosnia and Herzegovina", "387"},
			{"Botswana", "267"},
			{"Brazil", "55"},
			{"British Indian Ocean Territory", "246"},
			{"British Virgin Islands", "1284"},
			{"Brunei", "673"},
			{"Bulgaria", "359"},
			{"Burkina Faso", "226"},
			{"Burundi", "257"},
			{"Cambodia", "855"},
			{"Cameroon", "237"},
			{"Canada", "1"},
			{"Cape Verde", "238"},
			{"Cayman Islands", "345"},
			{"Central African Republic", "236"},
			{"Chad", "235"},
			{"Chile", "56"},
			{"China", "86"},
			{"Christmas Island", "61"},
			{"Cocos-Keeling Islands", "61"},
			{"Colombia", "57"},
			{"Comoros", "269"},
			{"Congo", "242"},
			{"Congo, Dem. Rep. of (Zaire)", "243"},
			{"Cook Islands", "682"},
			{"Costa Rica", "506"},
			{"Croatia", "385"},
			{"Cuba", "53"},
			{"Curacao", "599"},
			{"Cyprus", "537"},
			{"Czech Republic", "420"},
			{"Denmark", "45"},
			{"Diego Garcia", "246"},
			{"Djibouti", "253"},
			{"Dominica", "1767"},
			{"Dominican Republic", "1809"},
			{"East Timor", "670"},
			{"Easter Island", "56"},
			{"Ecuador", "593"},
			{"Egypt", "20"},
			{"El Salvador", "503"},
			{"Equatorial Guinea", "240"},
			{"Eritrea", "291"},
			{"Estonia", "372"},
			{"Ethiopia", "251"},
			{"Falkland Islands", "500"},
			{"Faroe Islands", "298"},
			{"Fiji", "679"},
			{"Finland", "358"},
			{"France", "33"},
			{"French Antilles", "596"},
			{"French Guiana", "594"},
			{"French Polynesia", "689"},
			{"Gabon", "241"},
			{"Gambia", "220"},
			{"Georgia", "995"},
			{"Germany", "49"},
			{"Ghana", "233"},
			{"Gibraltar", "350"},
			{"Greece", "30"},
			{"Greenland", "299"},
			{"Grenada", "1473"},
			{"Guadeloupe", "590"},
			{"Guam", "1671"},
			{"Guatemala", "502"},
			{"Guinea", "224"},
			{"Guinea-Bissau", "245"},
			{"Guyana", "595"},
			{"Haiti", "509"},
			{"Honduras", "504"},
			{"Hong Kong SAR China", "852"},
			{"Hungary", "36"},
			{"Iceland", "354"},
			{"India", "91"},
			{"Indonesia", "62"},
			{"Iran", "98"},
			{"Iraq", "964"},
			{"Ireland", "353"},
			{"Israel", "972"},
			{"Italy", "39"},
			{"Ivory Coast", "225"},
			{"Jamaica", "1876"},
			{"Japan", "81"},
			{"Jordan", "962"},
			{"Kazakhstan", "77"},
			{"Kenya", "254"},
			{"Kiribati", "686"},
			{"Kuwait", "965"},
			{"Kyrgyzstan", "996"},
			{"Laos", "856"},
			{"Latvia", "371"},
			{"Lebanon", "961"},
			{"Lesotho", "266"},
			{"Liberia", "231"},
			{"Libya", "218"},
			{"Liechtenstein", "423"},
			{"Lithuania", "370"},
			{"Luxembourg", "352"},
			{"Macau SAR China", "853"},
			{"Macedonia", "389"},
			{"Madagascar", "261"},
			{"Malawi", "265"},
			{"Maldives", "960"},
			{"Mali", "223"},
			{"Malta", "356"},
			{"Marshall Islands", "692"},
			{"Martinique", "596"},
			{"Mauritania", "222"},
			{"Mauritius", "230"},
			{"Mayotte", "262"},
			{"Mexico", "52"},
			{"Micronesia", "691"},
			{"Midway Island", "1808"},
			{"Moldova", "373"},
			{"Monaco", "377"},
			{"Mongolia", "976"},
			{"Montenegro", "382"},
			{"Montserrat", "1664"},
			{"Morocco", "212"},
			{"Myanmar", "95"},
			{"Namibia", "264"},
			{"Nauru", "674"},
			{"Nepal", "977"},
			{"Netherlands", "31"},
			{"Netherlands Antilles", "599"},
			{"Nevis", "1869"},
			{"New Caledonia", "687"},
			{"New Zealand", "64"},
			{"Nicaragua", "505"},
			{"Niger", "227"},
			{"Nigeria", "234"},
			{"Niue", "683"},
			{"Norfolk Island", "672"},
			{"North Korea", "850"},
			{"Northern Mariana Islands", "1670"},
			{"Norway", "47"},
			{"Oman", "968"},
			{"Pakistan", "92"},
			{"Palau", "680"},
			{"Palestinian Territory", "970"},
			{"Panama", "507"},
			{"Papua New Guinea", "675"},
			{"Paraguay", "595"},
			{"Peru", "51"},
			{"Philippines", "63"},
			{"Poland", "48"},
			{"Portugal", "351"},
			{"Puerto Rico", "1787"},
			{"Qatar", "974"},
			{"Reunion", "262"},
			{"Romania", "40"},
			{"Russia", "7"},
			{"Rwanda", "250"},
			{"Samoa", "685"},
			{"San Marino", "378"},
			{"Saudi Arabia", "966"},
			{"Senegal", "221"},
			{"Serbia", "381"},
			{"Seychelles", "248"},
			{"Sierra Leone", "232"},
			{"Singapore", "65"},
			{"Slovakia", "421"},
			{"Slovenia", "386"},
			{"Solomon Islands", "677"},
			{"South Africa", "27"},
			{"South Georgia and the South Sandwich Islands", "500"},
			{"South Korea", "82"},
			{"Spain", "34"},
			{"Sri Lanka", "94"},
			{"Sudan", "249"},
			{"Suriname", "597"},
			{"Swaziland", "268"},
			{"Sweden", "46"},
			{"Switzerland", "41"},
			{"Syria", "963"},
			{"Taiwan", "886"},
			{"Tajikistan", "992"},
			{"Tanzania", "255"},
			{"Thailand", "66"},
			{"Timor Leste", "670"},
			{"Togo", "228"},
			{"Tokelau", "690"},
			{"Tonga", "676"},
			{"Trinidad and Tobago", "1868"},
			{"Tunisia", "216"},
			{"Turkey", "90"},
			{"Turkmenistan", "993"},
			{"Turks and Caicos Islands", "1649"},
			{"Tuvalu", "688"},
			{"U.S. Virgin Islands", "1340"},
			{"Uganda", "256"},
			{"Ukraine", "380"},
			{"United Arab Emirates", "971"},
			{"United Kingdom", "44"},
			{"United States", "1"},
			{"Uruguay", "598"},
			{"Uzbekistan", "998"},
			{"Vanuatu", "678"},
			{"Venezuela", "58"},
			{"Vietnam", "84"},
			{"Wake Island", "1808"},
			{"Wallis and Futuna", "681"},
			{"Yemen", "967"},
			{"Zambia", "260"},
			{"Zanzibar", "255"},
			{"Zimbabwe", "263"}
		};
	}
}
