# ReadingRainbow
Mobile Application for Sharing and Managing Bookshelf

Reading Rainbow Standalone Front-End that was developed while the API and Sign-In features were in progress. Thus, the code utilizes the Google Books API to imitate calls that should be replaced by calls to the Reading Rainbow API. 

This WIP front end focuses primarily on React Native Navigation, routing of Parameters, conditional rendering of portraits and books, Cloudinary Portrait uploads, and Book genre searches via BISAC IDs utilizing Google Books API. Therefore, this standalone chunk of code may serve as a useful user experience prototype for the completed project. 

To run, copy and paste the contents of App_updated_jan2021.js to your local App.js of your React Native project.

You will need to install React-Native CLI Quickstart (https://reactnative.dev/docs/environment-setup) and Android Studio  (https://developer.android.com/studio).

Reading Rainbow utilizes Cloudinary (a cloud-based hosting service) and React-Native ImagePicker for User Portrait Uploads. A Cloudinary Name and Upload Preset is provided with the repository.

To install ImagePicker, navigate to your project repository and in the editor terminal, execute the following:
$ expo install expo-image-picker

To create a Free Cloudinary Account, navigate to https://cloudinary.com/users/register/free and follow the registration instructions.

Open the project in Android Studios, initialize the Android Virtual Device Manager, and run an Android Smartphone emulator of your choice. 
In your terminal, run $ npm start  to initialize the development server (i.e. http://localhost:19002/) and click on “Run on Android Device/Emulator”. This should initialize the app on your selected Smartphone Emulator. 

Known Issues: Google Books API may sometimes return results that lack a Title Image/Book Thumbnail. This will cause the front end to crash. May need to upload placeholder book image to Cloudinary and insert JavaScript ternary operator if image URI is null.

