What's included:

The Release folder contains the .exe of the kata (with its own console) and a testing .dll named 'Kata_Checkout_Test.dll'.

How To Test:
run Developer Command Prompt for Visual Studio.
change directory to where the project is (preferably within the Release folder)
run vstest.console.exe with the .dll file.

Copy/Pasteable command:
vstest.console.exe Kata_checkout_test.dll

(I could run vstest.console.exe without navigating to anything, so unless it literally involves install visual studio I think it should work fine as is.)

How to Run the Kata_Checkout.exe:
run it?
All available documentation are present inside the console.