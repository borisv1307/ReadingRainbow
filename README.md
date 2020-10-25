# ReadingRainbow
Mobile Application for Sharing and Managing Bookshelf

Tl;dr Version:

First, Set up Neo4j Environment - Recommend Installing Neo4j desktop to host a localized database for testing purposes. 
Upon starting Neo4j Desktop, Create a local database (1234 or whatever password) and Import Neo4jDB.csv file. Be sure to copy the File URL or Local File Path.

Click RUN and Open in Neo4j Browser. This should give you a prompt. Copy File URL (Or file Path and Use file:/// prefix before file name) to clipboad and insert into LOAD CSV statement.

LOAD CSV WITH HEADERS FROM 'http://localhost:11001/project-32e06daf-0ca3-47a0-91f0-8b4cc74c1ebc/Neo4jDB.csv' AS line
MERGE (p1:Person { name: line.Unique_Name, profile: line.About_Me, portrait: line.Profile_Img_URL })
MERGE (p2:Person { name: line.F_Unique_Name, profile: line.F_About_Me, portrait: line.F_Profile_Img_URL })
MERGE (b1:Book { id: line.L_Book_ID, title: line.L_B_Title, author: line.L_B_Authors, thumbnail: line.L_B_Thumbnail, publishdate: line.L_B_Publish_Date, pagenum: line.L_B_Pages, description: line.L_B_Description, isbn10: line.L_B_ISBN_10, isbn13: line.L_B_ISBN_13 })
MERGE (b2:Book { id: line.W_Book_ID, title: line.W_B_Title, author: line.W_B_Authors, thumbnail: line.W_B_Thumbnail, publishdate: line.W_B_Publish_Date, pagenum: line.W_B_Pages, description: line.W_B_Description, isbn10: line.W_B_ISBN_10, isbn13: line.W_B_ISBN_13 })
MERGE (p1)-[f:FRIENDS_WITH]->(p2)
MERGE (p1)-[w:WISH_LISTS]->(b2)
MERGE (p1)-[l:IN_LIBRARY]->(b1)

MATCH (p:Person), (b:Book) WHERE p.name="Frederica Greenhill" AND b.id="JZpeDwAAQBAJ"
CREATE (p)-[r:Recommend_B]->(b)
RETURN p,b

CREATE (p:Person{name:"Dusty Attenborough" , profile:"FPA Commodore" , portrait:"URL"})
RETURN p

MATCH (p:Person), (b:Book) WHERE p.name="Dusty Attenborough" AND b.id="4KwCoQEACAAJ"
CREATE (p)-[r:WISH_LISTS]->(b)
RETURN p,b

MATCH (p:Person), (b:Book) WHERE p.name="Dusty Attenborough" AND b.id="5NomkK4EV68C"
CREATE (p)-[r:IN_LIBRARY]->(b)
RETURN p,b

MATCH (p:Person), (b:Book) WHERE p.name="Dusty Attenborough" AND b.id="fdSJDQAAQBAJ"
CREATE (p)-[r:IN_LIBRARY]->(b)
RETURN p,b

(Use the RETURN statement to confirm new nodes/relations have been created - otherwise proceed)

To view Database Schema, Use CALL db.schema.visualization() - it should be merely Persons and Books with the relations FRIENDS_WITH, WITH_LISTS, IN_LIBRARY and Recommend_B.
To View entirety of database (not recommended for Large databases) use some catchall local variables. 
MATCH(p1)-[w]->(b1)
RETURN p1,w,b1

Log 10/25/2020 
Hit Major Roadblock with Google Books API and instead proceeded with recommendation algorithms centered around Neo4j database. Currently recommendations based on novels that friends recommend, favorite author and Jaccard Indexes (concerning the similarity between User Libraries and User Wish Lists) are implemented, functional and tested with this startup Database. More expansive testing and more testing parameters are necessary to proceed further. 

Currently facing problems with null data, database design concerning uni- and bi-directional relationships, and special/unusual characters from Google Books API returns. Recommendation algorithms will undoubtedly have duplicate recommendations due to the sheer number of books and how only pairs of libraries are directly compared - this may be mitigated via other layers. Neo4j cypher does not fare well thus far with nodes with numerous properties (such as those for books) or perhaps this is due to inexperience on my part. 
