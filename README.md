# ReadingRainbow
While Netflix provides customizes recommendations for you and Amazon tailors it’s advertising to your every click, there isn’t an app quite yet that works with in-depth algorithms to provide you with the title of your next great read. Reading Rainbow is designed to provide its book-loving users with an accessible place to find new book recommendations based on the user’s interests. It also encourages users to friend each other and request to borrow books from their physical libraries. Theoretically, this app could be monetized through paid ads for certain books and through directing users to purchasing book recommendation at an online retailer. 

## Work Completed 

We used Visual Studio Code as our IDE, and highly recommend that you use it too. 

### Front-End - React Native framework (JavaScript) using the Expo SDK 

Sign-Up/Create Account - Fully implemented 

Sign-In - Fully implemented 

Logout - Fully implemented 

JSON Web Token (jwt) for authentication – Fully implemented 

Forgot Password – API groundwork available, not implemented in the front-end. 

User Interface – Implemented at a basic level; needs to be evaluated and redesigned for UX and accessibility. 

Search for books (using Google Books API) - fully implemented 

Display Book Info – Mostly implemented. Some details of book not fully outlined. 

Return Profile & Library info 

Upload Picture – Default profile picture implemented, Cloudinary upload (users uploading their own images) mocked but not fully integrated into dev branch 

Navigation - Fully implemented, including the rootstack screen when logged out and the full screen stack when logged in. 

AddFriendRequest - Implementation begun 

Reading Rainbow utilizes Cloudinary (a cloud-based hosting service) and React-Native ImagePicker for User Portrait Uploads. While a Cloudinary Name and Upload Preset is provided with the repository, it may be useful to create your own Account to modify Image Upload Presets, manage Images and so forth. The implementation for this has begun on the standalone_frontend_via_GoogleAPI branch, but never fully implemented into the dev branch. 

To install ImagePicker, navigate to your project repository and in the editor terminal, execute the following: 

$ expo install expo-image-picker 

To create a Free Cloudinary Account, navigate to https://cloudinary.com/users/register/free and follow the registration instructions.  

After registering an account, Login and navigate to Settings Icon (top right of dashboard) and click on “Upload Presets”. Create a new Upload Preset (i.e., Unsigned, Public, Upload) and note the following: 

Cloud Name from the Dashboard:  

Upload Preset Name: 

Cloudinary API Endpoint – the Cloudinary Endpoint should be in the form https://api.Cloudinary.com/v1_1/:cloud_name/:action 

In this case, https://api.Cloudinary.com/v1_1/:cloud_name/image/upload where :cloud_name is obtained from your Dashboard. 

Install Cloudinary SDK: 

npm install cloudinary-react –save 

In the Code, replace the following with your Cloud Name, Cloudinary API Endpoint and Upload Preset Name where indicated: 

  const cloudinaryUpload = (pic) => { 

      const data = new FormData() 

      data.append('file', pic) 

      data.append('upload_preset', “UPLOAD_PRESET_NAME”) 

      data.append("cloud_name", “CLOUDINARY_NAME”) 

      setLoading(true) 

      fetch('CLOUDINARY_API_ENDPOINT', 

          { 

            method: 'POST', 

            body: data 

          }) 

          .then(res => res.json()) 

          .then(data => {setImage(data.secure_url)}) 

          .catch(err => {Alert.alert("Uploading Error")}) 

      setLoading(false) 

      } 

… 

### Middle-layer/API (C# .NET Core) 

The API is divided into major sections the DAL (data access layer), the models, middleware and the controllers. The tests written in the ReadingRainbowAPI.Tests project are separated by test goal. The repository tests test repository functionality and require the Neo4j Database to be running. The controller tests moq the repository functions so the database does not need to be running for these tests (but can be running).  

There are a few endpoints that have not been implemented and some implemented endpoints that require minor modifications. 

Endpoints still needed: 

Get Wishlist – per username getting all books in the user’s wishlist 
Get Friend’s profile – per username and friend’s username get friend's profile. Needs to validate user and friend have a friend relationship in both directions. 
Get Friend’s Library – per username and friend’s username get friend's library. Needs to validate user and friend have a friend relationship in both directions. 
Get Friend’s Wishlist – per username and friend’s username get friend's wishlist. Needs to validate user and friend have a friend relationship in both directions. 
Get Recommendations based on Jaccard Index – not functional likely due to errors in modeling Neo4j Cypher queries in C#.  

Existing Endpoint modifications: 
The Sign In function (Endpoint: api/token) should be updated to allow user to sign in. with email address instead of username 
The find user function (endpoint: api/person/People) should be updated to include the username – to not return the user requesting the list of people, and should only return a limited number of entries at a time. It would not be feasible to return all the people in the database to the front end at once since that would not be scalable. There should also be some sort of search functionality implemented in it.  
The ResetPassword function (endpoint: api/person/ResestPassword) requires a person object to be passed to the function. This was done so the no url encoding/decoding is performed on the email address entered. The person object passed to the function only needs the Email string entered.  
 
### Database (Neo4J Graph Database, Cypher Query Language) 

Design & implementation completed

## Next Steps 
Fully implement the interfaces outlined in the Interfaces document. 
Sign in with other accounts (such as Apple, Google, or FaceBook). 
Additionally, please see the Projects tab on GitHub for our To-Do's on the Kanban board. 

## Front-End Setup 
Install Node.js at https://nodejs.org/en/  
Download Android Studio https://developer.android.com/studio and subsequently set up a mobile android device. 
Install Expo SDK https://expo.io/ 

## API Setup 
Terminal commands to install necessary packages  
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 5.0.0 
dotnet add package AutoMapper --version 10.1.1 
dotnet add package RSG.Promise --version 3.0.1 
dotnet add package System.IdentityModel.Tokens.Jwt --version 6.8.0 
dotnet add package Neo4j.Driver --version 4.2.0 
dotnet add package Neo4jClient --version 4.0.3 
dotnet add package MailKit --version 2.11.0 

Appsettings.json File 
Both the ReadingRainbowAPI and ReadingRainbowAPI.Tests require an Appsettings.json file. This file is used for configurable settings for the application.  

Some notable Sections: 

1. JwtConfig – this is used for generating the jwt token to send back to the front end ReadingRainbow application after the user has successfully signed in. Any endpoint that is not marked with [AllowAnonymous] will require the token to be in the request. 

2. Ne04jConnectionSettings – this is for the Neo4j database connection. The username can be the administrative user setup when the database was created or a separate user that is created. If creating a separate user make sure that user has the correct permissions for creating, updating, deleting and querying nodes and relationships.  

3. Email – See below on Setup for email functions.  The tokenHours section indicates the number of hours the user has after signup to verify their email (click the link in the email). –24 hours here means that during email confirmation the API will validate the token was requested within the past 24 hours. 

4. PasswordReset – this section is for how long the user has after requesting their password to reset the password. -1 hour here means that when the user requests a password reset they have 1 hour to use the new password to login to the readingrainbow application and change their password.  

Setup for email functions 

In order to use the email service, you can use your google account (this will send emails using the google smtp server) and create an app password. In a production environment this would not be a good solution but for the purposes of development and testing it was acceptable. Instructions to setup the app password for a google account are below in the link. These values are updated in the appsettings.json file, under the Email Section. Make sure to not check your app password into source control. An app password is needed to bypass 2 factor authentication, so an alternative to this is to use an smtp server that does not require 2 factor authentication.  

Setting up Google App Password: https://support.google.com/accounts/answer/185833?hl=en#:~:text=Create%20%26%20use%20App%20Passwords%201%20Go%20to,Advanced%20Protection%20for%20your%20account.%20More%20items%20 

 

## API Tests Setup 
Terminal commands to install necessary packages 
dotnet add package AutoMapper --version 10.1.1 
dotnet add package Moq --version 4.15.1  
dotnet add package Microsoft.NET.Test.Sdk --version 16.9.1 

Appsettings.json File 

See the description for the Appsettings.json file in the ReadingRainbowAPI setup instructions. These files are almost identical. The only differences are the neo connection settings are under appSettings (and parameters are named slightly different) and the Email section includes a TestSendToEmail. The TestSendToEmail  variable is for one of the email tests where the test sends an email to the TestSendToEmail referenced here and validates it was successfully sent.  

 

## Database Setup (Neo4j) 

Download/install Neo4j Desktop 4.2 - https://neo4j.com/download/ 
Create a new local database 
Create new user and assign admin role - Documentation 
Replace credentials in appsetting.json in VS Code 
Population and algorithm documentation – Neo4j-Database+Algorithms github branch 
Populate with nodes and relationships - Neo4jCypher_Expanded.txt 
Population csv – Neo4jDB.csv 
Start database before running code 
Reading Rainbow utilizes Neo4j, an open-source graph database, for storing Persons, Books, and the relationships between them.   
First, install Neo4j desktop from https://neo4j.com/download/ 
Follow instructions for registration and retrieve product key for Neo4j Desktop App activation.  
Run Neo4j Desktop, proceed to create New Project and Add a local DBMS to get started.  Record the password of your local DBMS and Modify the Password Settings in the Github code.  
Open appsettings.json and modify the following with your Neo4j Username and Neo4j DBMS password (or alternatively, set your Local Neo4j settings to the following): 
    "appSettings" : 

    { 

        "neoLocalUserName": "neo4j", 

        "neoLocalPassword": "1234", 

        "neoDevUrl" : "http://localhost:7474" 

    }, 
Start your local Neo4j DBMS and Open with Neo4j Browser. By default, you should have been assigned an admin role with your Local DBMS. You may create new user(s) and assign admin role(s)- Documentation. 
You may create nodes and relationships individually with Cypher but for the sake of brevity, it may be best to import CSV files with detailed nodes.  
Reading Rainbow Database consists of two types of nodes; Person and Books, and 4 types of relationships; friends with, wish listed, in library, and recommend (book). At this time, friends recommending books to other friends have not been set up. Where implicit (i.e. friends), relationships are bi-directional. 
Import Neo4jDB.csv file from Github Neo4j-Database+Algorithms branch into local Neo4j Desktop and copy the local file path. Enter the following Cypher Code in Neo4j Browser Terminal. Be sure to use your local file path.  
LOAD CSV WITH HEADERS FROM 'http://localhost:11001/project-32e06daf-0ca3-47a0-91f0-8b4cc74c1ebc/Neo4jDB.csv' AS line MERGE (p1:Person { Name: line.Unique_Name, Profile: line.About_Me, Portrait: line.Profile_Img_URL }) MERGE (p2:Person { Name: line.F_Unique_Name, Profile: line.F_About_Me, Portrait: line.F_Profile_Img_URL }) MERGE (b1:Book { Id: line.L_Book_ID, Title: line.L_B_Title, Author: line.L_B_Authors, Thumbnail: line.L_B_Thumbnail, PublishDate: line.L_B_Publish_Date, PageNum: line.L_B_Pages, Description: line.L_B_Description, ISBN10: line.L_B_ISBN_10, ISBN13: line.L_B_ISBN_13 }) MERGE (b2:Book { Id: line.W_Book_ID, Title: line.W_B_Title, Author: line.W_B_Authors, Thumbnail: line.W_B_Thumbnail, PublishDate: line.W_B_Publish_Date, PageNum: line.W_B_Pages, Description: line.W_B_Description, ISBN10: line.W_B_ISBN_10, ISBN13: line.W_B_ISBN_13 }) MERGE (p1)-[f:FRIENDS_WITH]->(p2) MERGE (p1)-[w:WISH_LISTS]->(b2) MERGE (p1)-[l:IN_LIBRARY]->(b1) 

This should provide a rudimentary database. A much more expanded database (with more Books and Persons) is provided with Neo4jDB_expansive_Testing_v1.4.csv file.  Follow essentially the same instructions for the expanded csv file as above.  

Ideally, a Person Node would have the following: 

//If said individual is not in the database, use MERGE instead of MATCH. 
MATCH (p:Person {Name: "Reinhard von Lohengramm"}) 
SET p.Email = 'Lohengramm@gmail.com' 
SET p.HashedPassword = '9f735e0df9a1ddc702bf0a1a7b83033f9f7153a00c29de82cedadc9957289b05' 
SET p.Portrait = 'https://res.cloudinary.com/dotcpqooc/image/upload/v1610872168/iLportrait/or3d1pxfpfzdjfcfoq4x.jpg'  
SET p.EmailConfirmed = 'True' 
RETURN p 

Note, all users in this Neo4j example database are fictional and taken from Legend of the Galactic Heroes. Most user nodes do not have portraits, email, passwords, and such prepared and will need to be updated accordingly (e.g. repeat the aforementioned commands for each user name). 

For instance: 

MERGE (p:Person {Name: "Julian Minci"}) 
SET p.Email = 'Minci@gmail.com' 
SET p.HashedPassword = '9f735e0df9a1ddc702bf0a1a7b83033f9f7153a00c29de82cedadc9957289b05' 
SET p.Portrait = 'https://res.cloudinary.com/dotcpqooc/image/upload/v1610872176/iLportrait/x05z6oy1b3jlnzyxydlz.jpg' 
SET p.EmailConfirmed = ‘True’ 

RETURN p 

MERGE (p:Person {Name: "Yang Wen-Li"}) 
SET p.Email = 'Yang@gmail.com' 
SET p.HashedPassword = '9f735e0df9a1ddc702bf0a1a7b83033f9f7153a00c29de82cedadc9957289b05' 
SET p.Portrait = 'https://res.cloudinary.com/dotcpqooc/image/upload/v1610872155/iLportrait/dllyvhqkv1remxoyvqjq.jpg' 
SET p.EmailConfirmed = ‘True’ 

RETURN p 

Several Cypher queries and Algorithms are provided in the Neo4jCypher_Algorithms_WiP.txt file. 

Before running the code via Expo CLI, be sure to initialize the database. 
