# ReadingRainbow
Mobile Application for Sharing and Managing Bookshelf

ui-screen+navigation 

Log: 10/11/2020

17 classes created to render screens. Rudimentary navigation between screens are functional but not the most user friendly (due mostly to inability to present friend list names from flatlist).

Current Issues that need resolving:
Drawer functionality is not working. Drawer navigation, icons, buttons, and the such are implemented by not functional. Suspect the drawer navigation/class must be moved to seperate file and exported to work.
Friendlist features are not functional due to changes in functional hooks. Flatlist no longer can take useState functions and must be revised. 
Preferences Page: class and code is written but not implemented at this time. 

Addendum: setNames, setPasswords not functional due to lack of middle layer at this time - mostly used as reminder. Need to comment out. 
After group discussion, Search and Browse functionality should be merged into one.

Log: 10/12/2020

WIP friendlist navigation is functional. Search functionality merged into Browse functionality and each Class have been modified to reflect revision. 
Initial Preferences page implemeneted but lacking "genres" due to unavailability of "category" information from the Google Library API (middle layer team is currently exploring Google API).
Scrollviews and Flex functions for images partially implemented. This will largely depend on the size of thumbnails obtained from Google Library API. 
Proper navigation implemented to link new classes and remove old classes. 
