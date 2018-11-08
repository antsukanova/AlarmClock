# AlarmClock
MVVM based WPF application for creating and setting alarm clocks for different users.

## How to use
To run the application - just clone the repository itself and run through Visual Studio.
After starting the application, you have to sign in with your own data or create a new account. 
After the registration procedure, private cabinet appears where user can set alarm clocks.

| Buttons | Usage |
|---|---|
| ↑ | Scroll(for choosing new time and edit added time) hours/minutes up |
| ↓ | Scroll hours/minutes down (for choosing new time and edit added time) |
| + | Add new clock |
| 🔔 | Activte alarm clock |
| ╳ | Delete alaram clock |
| 🚪 | Sign out |

If the add button has a gray color, it means that the list of alarm clocks already has the alarm for the same time and it is impossible to add another one.
The same situation occures when you are editing an alarm clock.

If you want to start alarm clock ringing manually - you have to click on the bell icon to the right of the time and it will blink for 2 minutes or untill you press the button again.

## Tests
Used technologies - xUnit.net, White, Fluent Assertions.

Folder `tests` contains UI tests of Sign In and Sign Up functional.
Tests for different files should be run separately, because they conflict with each other for some reason.
Your keyboard layout should be set to English, because White uses it to enter text.

To run tests via Visual Studio you can open a test file, click on the icon next to the class name and choose "Run all".
Other IDEs should have simillar ways to do that.

## Authors
Tsukanova Anna and Morenets Igor.
