# ReadingRainbow
Mobile Application for Sharing and Managing Bookshelf

Tl;dr Version:

First, Set up Neo4j Environment - Recommend Installing Neo4j desktop to host a localized database for testing purposes. 
Upon starting Neo4j Desktop, Create a local database (1234 or whatever password) and Import Neo4jDB.csv file. Be sure to copy the File URL or Local File Path.

Click RUN and Open in Neo4j Browser. This should give you a prompt. Copy File URL (Or file Path and Use file:/// prefix before file name) to clipboad and insert into LOAD CSV statement.

LOAD CSV WITH HEADERS FROM 'http://localhost:11001/project-32e06daf-0ca3-47a0-91f0-8b4cc74c1ebc/Neo4jDB.csv' AS line
MERGE (p1:Person { Name: line.Unique_Name, Profile: line.About_Me, Portrait: line.Profile_Img_URL })
MERGE (p2:Person { Name: line.F_Unique_Name, Profile: line.F_About_Me, Portrait: line.F_Profile_Img_URL })
MERGE (b1:Book { Id: line.L_Book_ID, Title: line.L_B_Title, Author: line.L_B_Authors, Thumbnail: line.L_B_Thumbnail, PublishDate: line.L_B_Publish_Date, PageNum: line.L_B_Pages, Description: line.L_B_Description, ISBN10: line.L_B_ISBN_10, ISBN13: line.L_B_ISBN_13 })
MERGE (b2:Book { Id: line.W_Book_ID, Title: line.W_B_Title, Author: line.W_B_Authors, Thumbnail: line.W_B_Thumbnail, PublishDate: line.W_B_Publish_Date, PageNum: line.W_B_Pages, Description: line.W_B_Description, ISBN10: line.W_B_ISBN_10, ISBN13: line.W_B_ISBN_13 })
MERGE (p1)-[f:FRIENDS_WITH]->(p2)
MERGE (p1)-[w:WISH_LISTS]->(b2)
MERGE (p1)-[l:IN_LIBRARY]->(b1)

MATCH (p:Person), (b:Book) WHERE p.Name="Frederica Greenhill" AND b.Id="JZpeDwAAQBAJ"
CREATE (p)-[r:RECOMMEND_B]->(b)
RETURN p,b

CREATE (p:Person{Name:"Dusty Attenborough" , Profile:"FPA Commodore" , Portrait:"URL"})
RETURN p

MATCH (p:Person), (b:Book) WHERE p.Name="Dusty Attenborough" AND b.Id="4KwCoQEACAAJ"
CREATE (p)-[r:WISH_LISTS]->(b)
RETURN p,b

MATCH (p:Person), (b:Book) WHERE p.Name="Dusty Attenborough" AND b.Id="5NomkK4EV68C"
CREATE (p)-[r:IN_LIBRARY]->(b)
RETURN p,b

MATCH (p:Person), (b:Book) WHERE p.Name="Dusty Attenborough" AND b.Id="fdSJDQAAQBAJ"
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

Log 11/01/2020
Expanded testing database by nearly 5 folds to 103 nodes (of 2 types) and 240 relationships (of 4 types) with intersecting user libraries and wish lists to facilite more realistic and practical testing conditions. See txt file with accompanying csv file for instructions on how to append to existing Neo4j test library. 

Where appropriate (and implicit based on the nature of the relationship), bi-directional relationships at the query level are implemented. 

Popularity indexing (ie. highly wish listed book! or most popular books users are reading!) based on the nominal number of relationships to specific book nodes have been implemented. Rather than a recommendation algorithm, this is done to create a sense of group and solidarity amongst our users.

For the most part, previously constructed algorithms and recommendation cypher queries work as intended; however issues with duplicate recommendations inevitably arose. Furthermore, due mostly to design choices, wish lists and libraries are kept seperate, meaning it is possible to recommend a book for wish lists when said user already have book in library. 

Duplicate Recommendations from Different Friends were fairly simple to resolve due to the localization of cypher variables. 
However, duplicate Recommendations in Recommendations based on Jaccard indexing are mostly due to nature of Jaccard Index: Assume Users A,B,C all have similar books. User B and User C may have the same books that User A do not have and therefore Jaccard Index would recommend the same book (from B and C) to User A twice. This will likely be resolved at the front end (via React Native Rendering).

Furthermore, since Jaccard index is based on WISH_LISTS OR LIBRARY, it does not take into account books that exists in the user's respective library/wishlist and may recommend books already in User A's library that is not in his wishlist and vice versa.

Regardless, it is unlikely these issues are significantly detrimental to user experience.
More critical is the matter of special characters or special character combinations (*,\,*\,\*) that return Neo4j errors. These remain unresolved at the API level. What to do with Null data remain unresolved. 

Log 11/05/2020
Successfully modified Jaccard Indexing Recommendation to take into account the complementary User B's Wish Lists/Library to ensure that books recommended to User A's library is not already in User A's Wish List and vice versa (i.e. book recommended to User A's Wish List is not already in User A's Library). This is due to the highly localized nature of Cypher Variables allowing for completely seperate queries to be made sequentially and carrying the local variables into further operations using WITH clause. Extensive testing was done with several intersecting wish lists and libraries and algorithms seems to be working correctly. 

NOTE: Neo4j graphical rendering capabilities are strangely glitchy and often miss a node (and the node's relationships) causing no small amount of panick. If numbers do not add up, simply re-run the query. 

Log 11/25/2020
Modified all Neo4j database related Cypher code to match proper Syntax.
