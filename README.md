# Cml.Data.PhoneNumber
A phone number library to auto to detect country code and phone number from a given input,
providing correct clean full phone number with digits only.

It detects common known country code from input, and allow separated value, either country code or number only to be extracted.
It also remove repeating country code from input, which is one of the common issues, 
  example: 65 65 8976 53xx
  which should be: +65 8976 53xx
  will be resolved to: +65897653xx

For Malaysia number, it will rectify one of the most common issue, the country code prepend to number with leading zero 0.
  example:              60 012 831 xxxx
  which should be:      +60 12 831 xxxx
  will be resolved to:  +6012831xxxx
