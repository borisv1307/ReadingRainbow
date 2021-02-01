import { ActivityIndicator, StyleSheet, Text, View, TextInput, Button, Image, FlatList, TouchableOpacity, ScrollView, Animated, Alert } from 'react-native';
import React, {useEffect, useState } from 'react';
import { DrawerActions, NavigationContainer } from '@react-navigation/native';
import { createStackNavigator } from '@react-navigation/stack';
import * as ImagePicker from 'expo-image-picker';

function RenderHomePage({navigation}) {
    const [isLoading, setLoading] = useState(true);
    const [wiRec, setWiRec] = useState([]);
    const [liRec, setLiRec] = useState([]);
    const [jaccardWiRec, setJaccardWiRec] = useState([]);
    const [jaccardLiRec, setJaccardLiRec] = useState([]);

    const styles = StyleSheet.create(
      {
      container: {
        flex: 1,
        backgroundColor: '#fff',
        alignItems: 'center',
        justifyContent: 'center',
      },
      buttonContainer: {
        marginTop: 20
      },
      input: {
        borderWidth: 1,
        borderColor: '#777',
        padding: 8,
        margin: 10,
        width: 200,
      },
      item: {
        marginTop: 10,
        padding: 10,
        backgroundColor: 'pink',
        fontSize: 12
      }
  });

//Need To pass User Data to home Screen from successful Sign In (namely, at least Username, Profile Info, and Image URL); a hard coded example (Neo4j JSON return is provided)
  const user = {
    "identity": 3,
    "labels": [
      "Person"
    ],
    "properties": {
      "name": "Frederica Greenhill",
      "portrait": "https://res.cloudinary.com/dotcpqooc/image/upload/v1605893230/iLportrait/faohavt36pvbexgmimcm.png",
      "profile": "FPA Sub-Lieutenant, Wife of Yang Wen-Li and later President of Iserlohn Republic"
    }
  }

  const name = user.properties.name;
  const portrait = user.properties.portrait;
  const profile = user.properties.profile;

//Need API calls to return a series of Recommendations (for instance, popular wish listed/in library books, Jaccard recs, Friend Recs)
  useEffect(() => {
      fetch('https://www.googleapis.com/books/v1/volumes?q=Lord of the Rings')
          .then((response) => response.json())
          .then((json) => setWiRec(json.items))
          .catch((error) => console.error(error))
          .finally(() => setLoading(false));
    }, []);
  
    useEffect(() => {
      fetch('https://www.googleapis.com/books/v1/volumes?q=The Chronicles of Narnia')
          .then((response) => response.json())
          .then((json) => setLiRec(json.items))
          .catch((error) => console.error(error))
          .finally(() => setLoading(false));
    }, []);   

    useEffect(() => {
        fetch('https://www.googleapis.com/books/v1/volumes?q=The Hunger Games')
            .then((response) => response.json())
            .then((json) => setJaccardWiRec(json.items))
            .catch((error) => console.error(error))
            .finally(() => setLoading(false));
      }, []);

    useEffect(() => {
        fetch('https://www.googleapis.com/books/v1/volumes?q=Anne McCaffrey')
            .then((response) => response.json())
            .then((json) => setJaccardLiRec(json.items))
            .catch((error) => console.error(error))
            .finally(() => setLoading(false));
      }, []);  


    return (
        <View style={styles.container}>
            {isLoading ? <ActivityIndicator/> : (
                <ScrollView>
                <TouchableOpacity onPress={()=>{navigation.navigate ('Profile', {
                    name: user.properties.name,
                    portrait: user.properties.portrait,
                    profile: user.properties.profile,               
                })}}>
                  <Text>My Profile</Text>
                </TouchableOpacity>
                <Text>Welcome {name}!</Text>
                <Button
                    title='Search'
                    onPress={()=>{navigation.navigate ('Search for Books')}}
                />
                <Text>------Most Wish Listed Books!------</Text>
                <FlatList
                    horizontal = {true}
                    showsHorizontalScrollIndicator={true}
                    data={wiRec}
                    keyExtractor={({ id }, index) => id}
                    renderItem={({item}) => (
                    <TouchableOpacity onPress={()=>{navigation.navigate ('Book Information', {
                        title: item.volumeInfo.title,
                        author: item.volumeInfo.authors,
                        thumbnail: item.volumeInfo.imageLinks.thumbnail,
                        pubDate: item.volumeInfo.publishedDate,
                        pageNum: item.volumeInfo.pageCount,
                        description: item.volumeInfo.description,                   
                    })}}>
                        <View style = {styles.container}>
                        <Image source={{uri: item.volumeInfo.imageLinks.thumbnail}} style={{width: 128, height: 205}} />
                        </View>                  
                    </TouchableOpacity>  
                    )}
                />
                <Text>------Most Popular Books In User Libraries------</Text>
                <FlatList
                    horizontal = {true}
                    showsHorizontalScrollIndicator={true}
                    data={liRec}
                    keyExtractor={({ id }, index) => id}
                    renderItem={({item}) => (
                    <TouchableOpacity onPress={()=>{navigation.navigate ('Book Information', {
                        title: item.volumeInfo.title,
                        author: item.volumeInfo.authors,
                        thumbnail: item.volumeInfo.imageLinks.thumbnail,
                        pubDate: item.volumeInfo.publishedDate,
                        pageNum: item.volumeInfo.pageCount,
                        description: item.volumeInfo.description,                   
                    })}}>
                        <View style = {styles.container}>
                        <Image source={{uri: item.volumeInfo.imageLinks.thumbnail}} style={{width: 128, height: 205}} />
                        </View>                  
                    </TouchableOpacity>  
                    )}
                />
                <Text>------Recommendations based on your Wish List!------</Text>
                <FlatList
                    horizontal = {true}
                    showsHorizontalScrollIndicator={true}
                    data={jaccardWiRec}
                    keyExtractor={({ id }, index) => id}
                    renderItem={({item}) => (
                    <TouchableOpacity onPress={()=>{navigation.navigate ('Book Information', {
                        title: item.volumeInfo.title,
                        author: item.volumeInfo.authors,
                        thumbnail: item.volumeInfo.imageLinks.thumbnail,
                        pubDate: item.volumeInfo.publishedDate,
                        pageNum: item.volumeInfo.pageCount,
                        description: item.volumeInfo.description,                   
                    })}}>
                        <View style = {styles.container}>
                        <Image source={{uri: item.volumeInfo.imageLinks.thumbnail}} style={{width: 128, height: 205}} />
                        </View>                  
                    </TouchableOpacity>  
                    )}
                />
                <Text>------Recommended Books based on Your Library!------</Text>
                <FlatList
                    horizontal = {true}
                    showsHorizontalScrollIndicator={true}
                    data={jaccardLiRec}
                    keyExtractor={({ id }, index) => id}
                    renderItem={({item}) => (
                    <TouchableOpacity onPress={()=>{navigation.navigate ('Book Information', {
                        title: item.volumeInfo.title,
                        author: item.volumeInfo.authors,
                        thumbnail: item.volumeInfo.imageLinks.thumbnail,
                        pubDate: item.volumeInfo.publishedDate,
                        pageNum: item.volumeInfo.pageCount,
                        description: item.volumeInfo.description,                   
                    })}}>
                        <View style = {styles.container}>
                        <Image source={{uri: item.volumeInfo.imageLinks.thumbnail}} style={{width: 128, height: 205}} />
                        </View>                  
                    </TouchableOpacity>  
                    )}
                />
                </ScrollView>
            )}
        </View>
    );
}
//Profile Page - will need to take in a user parameter
//Can convert generic index to string keyExtractor={(item, index) => index.toString()}
function RenderProfile({route, navigation}) {
    const { name, portrait, profile } = route.params;
    const [isLoading, setLoading] = useState(true);
    const [data, setData] = useState([]);
    const [wlist, setWlist] = useState([]);
    const styles = StyleSheet.create(
      {
      container: {
        flex: 1,
        backgroundColor: '#fff',
        alignItems: 'center',
        justifyContent: 'center',
      },
      buttonContainer: {
        marginTop: 20
      },
      input: {
        borderWidth: 1,
        borderColor: '#777',
        padding: 8,
        margin: 10,
        width: 200,
      },
      item: {
        marginTop: 10,
        padding: 10,
        backgroundColor: 'pink',
        fontSize: 12
      }
  });
//Need correct API call to retrieve User Library and Wish List)
  useEffect(() => {
      fetch('https://www.googleapis.com/books/v1/volumes?q=lord of the rings')
          .then((response) => response.json())
          .then((json) => setData(json.items))
          .catch((error) => console.error(error))
          .finally(() => setLoading(false));
    }, []);
  
    useEffect(() => {
      fetch('https://www.googleapis.com/books/v1/volumes?q=The Chronicles of Narnia')
          .then((response) => response.json())
          .then((json) => setWlist(json.items))
          .catch((error) => console.error(error))
          .finally(() => setLoading(false));
    }, []);   

    return (
        <View style={styles.container}>
            {isLoading ? <ActivityIndicator/> : (
                <ScrollView>
                <Text>{name} Profile</Text>
                <TouchableOpacity onPress={()=>navigation.navigate('Upload Your Portrait!')}>
                  <Image source = {{uri:portrait}} style={{width: 400, height: 250}}/>
                </TouchableOpacity>
                <Text>About Me:</Text>
                <Text>{profile}</Text>
                <Text>------My Library------</Text>
                <FlatList
                    horizontal = {true}
                    showsHorizontalScrollIndicator={true}
                    data={data}
                    keyExtractor={({ id }, index) => id}
                    renderItem={({item}) => (
                    <TouchableOpacity onPress={()=>{navigation.navigate ('Selected Book Info', {
                        library: 'true',
                        wishlist: 'false',
                        title: item.volumeInfo.title,
                        author: item.volumeInfo.authors,
                        thumbnail: item.volumeInfo.imageLinks.thumbnail,
                        pubDate: item.volumeInfo.publishedDate,
                        pageNum: item.volumeInfo.pageCount,
                        description: item.volumeInfo.description,                   
                    })}}>
                        <View style = {styles.container}>
                        <Image source={{uri: item.volumeInfo.imageLinks.thumbnail}} style={{width: 128, height: 205}} />
                        </View>                  
                    </TouchableOpacity>  
                    )}
                />
                <Text>------Wish List------</Text>
                <FlatList
                    horizontal = {true}
                    showsHorizontalScrollIndicator={true}
                    data={wlist}
                    keyExtractor={({ id }, index) => id}
                    renderItem={({item}) => (
                    <TouchableOpacity onPress={()=>{navigation.navigate ('Selected Book Info', {
                        library: 'false',
                        wishlist: 'true',
                        title: item.volumeInfo.title,
                        author: item.volumeInfo.authors,
                        thumbnail: item.volumeInfo.imageLinks.thumbnail,
                        pubDate: item.volumeInfo.publishedDate,
                        pageNum: item.volumeInfo.pageCount,
                        description: item.volumeInfo.description,                   
                    })}}>
                        <View style = {styles.container}>
                        <Image source={{uri: item.volumeInfo.imageLinks.thumbnail}} style={{width: 128, height: 205}} />
                        </View>                  
                    </TouchableOpacity>  
                    )}
                />
                <Button
                    title='Add Books to Library or Wish List'
                    onPress={()=>{navigation.navigate ('Search for Books')}}
                />
                <Button
                    title='Friends'
                    onPress={()=>{navigation.navigate ('Friend List')}}
                />
                </ScrollView>
            )}
        </View>
    );
}


function UserBookInfo({route, navigation}) {
  const { wishlist, library, title, author, thumbnail, pubDate, pageNum, description } = route.params;
  const styles = StyleSheet.create(
    {
    container: {
      flex: 1,
      backgroundColor: '#fff',
      alignItems: 'center',
      justifyContent: 'center',
    },
    buttonContainer: {
      marginTop: 20
    },
    input: {
      borderWidth: 1,
      borderColor: '#777',
      padding: 8,
      margin: 10,
      width: 200,
    },
    item: {
      marginTop: 10,
      padding: 10,
      backgroundColor: 'pink',
      fontSize: 12
    }
});

  return (
      <View style = {styles.container}>
      <ScrollView>
          <Text>Title: {JSON.stringify(title)}</Text>
          <Text>Author: {JSON.stringify(author)}</Text>
          <Image source={{uri: thumbnail}} style={{width: 128, height: 205}}/>
          <Text>Publication Date: {JSON.stringify(pubDate)} </Text>
          <Text>Number of Pages: {JSON.stringify(pageNum)} </Text>
          <Text>Description: {JSON.stringify(description)}</Text>
          <View style={{flexDirection: 'row'}}>
          {library == 'true' 
              ? <Button
                  title='Remove from Library'
                  onPress={() =>{
                      alert('Novel Removed from Library');
                  }} />
              : <Button
                  title='Add to Library'
                  onPress={() =>{
                      alert('Novel added to Library!');
                  }}/> }
          {wishlist == 'true' 
              ? <Button
                  title='Remove from Wish List'
                  onPress={() =>{
                      alert('Novel Removed from Wish List');
                  }} />
              : null}  
          </View>
      </ScrollView>
      </View>
  )
}

//Generic Profile Page for other individuals - will need to take in a username parameter
//Can convert generic index to string keyExtractor={(item, index) => index.toString()}
function RenderGenericProfile({route, navigation}) {
    const { name, portrait, profile, friend } = route.params;
    const [isLoading, setLoading] = useState(true);
    const [data, setData] = useState([]);
    const [wlist, setWlist] = useState([]);
    const styles = StyleSheet.create(
      {
      container: {
        flex: 1,
        backgroundColor: '#fff',
        alignItems: 'center',
        justifyContent: 'center',
      },
      buttonContainer: {
        marginTop: 20
      },
      input: {
        borderWidth: 1,
        borderColor: '#777',
        padding: 8,
        margin: 10,
        width: 200,
      },
      item: {
        marginTop: 10,
        padding: 10,
        backgroundColor: 'pink',
        fontSize: 12
      }
  });

//Need API call for Other User's library and wish list and therefore require at least Username to be routed to function.
  useEffect(() => {
      fetch('https://www.googleapis.com/books/v1/volumes?q=lord of the rings')
          .then((response) => response.json())
          .then((json) => setData(json.items))
          .catch((error) => console.error(error))
          .finally(() => setLoading(false));
    }, []);
  
    useEffect(() => {
      fetch('https://www.googleapis.com/books/v1/volumes?q=The Chronicles of Narnia')
          .then((response) => response.json())
          .then((json) => setWlist(json.items))
          .catch((error) => console.error(error))
          .finally(() => setLoading(false));
    }, []);   

    return (
        <View style={styles.container}>
            {isLoading ? <ActivityIndicator/> : (
                <ScrollView>
                <Text>{name} Profile</Text>
                <Image source = {{uri:portrait}} style={{width: 400, height: 250}}/>
                <Text>About Me:</Text>
                <Text>{profile}</Text>
                <Text>------My Library------</Text>
                <FlatList
                    horizontal = {true}
                    showsHorizontalScrollIndicator={true}
                    data={data}
                    keyExtractor={({ id }, index) => id}
                    renderItem={({item}) => (
                    <TouchableOpacity onPress={()=>{navigation.navigate ('Book Information', {
                        title: item.volumeInfo.title,
                        author: item.volumeInfo.authors,
                        thumbnail: item.volumeInfo.imageLinks.thumbnail,
                        pubDate: item.volumeInfo.publishedDate,
                        pageNum: item.volumeInfo.pageCount,
                        description: item.volumeInfo.description,                   
                    })}}>
                        <View style = {styles.container}>
                        <Image source={{uri: item.volumeInfo.imageLinks.thumbnail}} style={{width: 128, height: 205}} />
                        </View>                  
                    </TouchableOpacity>  
                    )}
                />
                <Text>------Wish List------</Text>
                <FlatList
                    horizontal = {true}
                    showsHorizontalScrollIndicator={true}
                    data={wlist}
                    keyExtractor={({ id }, index) => id}
                    renderItem={({item}) => (
                    <TouchableOpacity onPress={()=>{navigation.navigate ('Book Information', {
                        title: item.volumeInfo.title,
                        author: item.volumeInfo.authors,
                        thumbnail: item.volumeInfo.imageLinks.thumbnail,
                        pubDate: item.volumeInfo.publishedDate,
                        pageNum: item.volumeInfo.pageCount,
                        description: item.volumeInfo.description,                   
                    })}}>
                        <View style = {styles.container}>
                        <Image source={{uri: item.volumeInfo.imageLinks.thumbnail}} style={{width: 128, height: 205}} />
                        </View>                  
                    </TouchableOpacity>  
                    )}
                />
                {friend == 'true' 
                    ? <Button
                        title='Remove from Friends List'
                        onPress={() =>{
                            alert('User Removed from Friends List');
                          }} />
                    : <Button
                        title='Add to Friends List'
                        onPress={() =>{
                            alert('User added to Friends List!');
                          }}/> 
                    }
                </ScrollView>
            )}
        </View>
    );
}
function BookInfo({route, navigation}) {
    const { title, author, thumbnail, pubDate, pageNum, description } = route.params;
    const styles = StyleSheet.create(
      {
      container: {
        flex: 1,
        backgroundColor: '#fff',
        alignItems: 'center',
        justifyContent: 'center',
      },
      buttonContainer: {
        marginTop: 20
      },
      input: {
        borderWidth: 1,
        borderColor: '#777',
        padding: 8,
        margin: 10,
        width: 200,
      },
      item: {
        marginTop: 10,
        padding: 10,
        backgroundColor: 'pink',
        fontSize: 12
      }
  });
  
    return (
        <View style = {styles.container}>
        <ScrollView>
            <Text>Title: {JSON.stringify(title)}</Text>
            <Text>Author: {JSON.stringify(author)}</Text>
            <Image source={{uri: thumbnail}} style={{width: 128, height: 205}}/>
            <Text>Publication Date: {JSON.stringify(pubDate)} </Text>
            <Text>Number of Pages: {JSON.stringify(pageNum)} </Text>
            <Text>Description: {JSON.stringify(description)}</Text>
            <View style={{flexDirection: 'row'}}>
            <Button
            title='Add to Wish List'
            onPress={() =>{
              alert('Book Added to Your Wish List!');
            }}
            />
            <Button
            title='Add to Library'
            onPress={() =>{
              alert('Book Added to Your Library');
            }}
            />           
            </View>
        </ScrollView>
        </View>
    )
}

//Need API call to update User's profile with the Image URL returned by Cloudinary.
function UploadImage({Navigation}) {
  const [image, setImage] = useState(null);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
      (async () => {
          const {status} = await ImagePicker.requestCameraRollPermissionsAsync();
          if (status !=='granted') {
              alert('Allow Access to Photo Library to Continue with Image Upload');
          }
      })();
  }, []);

  const selectImage = async () => {
      let selectedImage = await ImagePicker.launchImageLibraryAsync({
          mediaTypes: ImagePicker.MediaTypeOptions.Images,
          allowsEditing: true,
          quality: 0.5,
      });

      console.log(selectedImage);

      if(!selectedImage.cancelled) {
          const name = 'portrait';
          const type = 'image/*';
          const uri = selectedImage.uri;
          const source = {name, type, uri};
      console.log(source)
      cloudinaryUpload(source)
      }
  };

  const cloudinaryUpload = (pic) => {
      const data = new FormData()
      data.append('file', pic)
      data.append('upload_preset', 'iLportrait')
      data.append("cloud_name", "dotcpqooc")

      setLoading(true)
      fetch('https://api.cloudinary.com/v1_1/dotcpqooc/image/upload',
          {
            method: 'POST',
            body: data
          })
          .then(res => res.json())
          .then(data => {setImage(data.secure_url)})
          .catch(err => {Alert.alert("Uploading Error")})
      setLoading(false)
      }
  console.log(image)  
      return (
        <View style={{flex: 1, backgroundColor:'#fff', alignItems: 'center', justifyContent: 'center'}}>
          <Text>
            Upload Your Profile Picture!
          </Text>
          <Button title="Pick an image from camera roll" onPress={selectImage} />
          {loading ? (<ActivityIndicator />)
          : (<Image source={{ uri: image }} style={{ width: 200, height: 400 }} />)}
        </View>
      )
}

//GoogleSearch functionality may be moved to middle layer. However, atm middle layer does not povide means to search by genre.
function GoogleSearch({route, navigation}) {
    const {userInput} = route.params;
    const [isLoading, setLoading] = useState(true);
    const [data, setData] = useState([]);
    const styles = StyleSheet.create(
      {
      container: {
        flex: 1,
        backgroundColor: '#fff',
        alignItems: 'center',
        justifyContent: 'center',
      },
      buttonContainer: {
        backgroundColor: '#7a42f4',
        padding: 10,
        margin: 15,
        height: 40,
      },
      input: {
        borderWidth: 1,
        borderColor: '#777',
        padding: 8,
        margin: 10,
        width: 200,
      },
      item: {
        marginTop: 10,
        padding: 10,
        backgroundColor: 'pink',
        fontSize: 12
      }
    });
    useEffect(() => {
      fetch('https://www.googleapis.com/books/v1/volumes?q=' + userInput)
          .then((response) => response.json())
          .then((json) => setData(json.items))
          .catch((error) => console.error(error))
          .finally(() => setLoading(false));
    }, [userInput]);

    const ItemView = ({item}) => {
        return (
          <View style = {styles.container}>
          <TouchableOpacity onPress={()=>{navigation.navigate ('Book Information', {
            title: item.volumeInfo.title,
            author: item.volumeInfo.authors,
            thumbnail: item.volumeInfo.imageLinks.thumbnail,
            pubDate: item.volumeInfo.publishedDate,
            pageNum: item.volumeInfo.pageCount,
            description: item.volumeInfo.description,
          })}}>      
            <Text>Title: {item.volumeInfo.title} </Text>
            <Text>Author: {item.volumeInfo.authors}</Text>
            <Image source={{uri: item.volumeInfo.imageLinks.thumbnail}} style={{width: 128, height: 205}} />
            <Text>Publication Date: {item.volumeInfo.publishedDate} </Text>
            <Text>Number of Pages: {item.volumeInfo.pageCount} </Text>
            <Text>Description: {item.volumeInfo.description}</Text>
          </TouchableOpacity>
          </View>
        );
      };

    return (
        <View style={{ flex: 1, padding: 24}}>
            {isLoading ? <ActivityIndicator/> : (
                <FlatList
                    data={data}
                    keyExtractor={({ id }, index) => id}
                    renderItem={ItemView}
                />
            )}
        </View>
    );
}

function GenreSearch({navigation}) {
  const [search, setSearch] = useState('');
  const [filterBisacCodes, setFilterBisacCodes] = useState([]);
  const [bisacCodes, setBisacCodes] = useState([]);
  const [searchText, setSearchText] = useState('');
  const styles = StyleSheet.create({
    container: {
      backgroundColor: 'white',
    },
    itemStyle: {
      padding: 10,
    },
    textInputStyle: {
      height: 40,
      borderWidth: 1,
      paddingLeft: 20,
      margin: 5,
      borderColor: '#009688',
      backgroundColor: '#F0F8FF',
    },
  });
      
   React.useEffect(() => {
      fetch('https://raw.githubusercontent.com/KariFal56/ReadingRainbow/Neo4j-Database%2BAlgorithms/bisaccodes.json')
          .then((response) => response.json())
          .then((responseJson) => {
              setFilterBisacCodes(responseJson)
              setBisacCodes(responseJson);
              })
              .catch((error) =>{
                  console.error(error);
              });
          }, []);
      
      const searchBisacCodes = (text) => {
          if (text) {
              const data = bisacCodes.filter(
                  function (item) {
                      const categoryData = item.Category
                      ? item.Category.toUpperCase()
                      : ''.toUpperCase();
                  const textData = text.toUpperCase();
                  return categoryData.indexOf(textData) > -1;
                  }
              );
              setFilterBisacCodes(data);
              setSearch(text);
          } else {
            setFilterBisacCodes(bisacCodes);
            setSearch(text);
          }
      };

      const ItemView = ({item}) => {
          return (
            <TouchableOpacity onPress={()=>{navigation.navigate ('Search Results', {
              userInput: item.Code,
            })}}>
              <Text style={styles.itemStyle}>
                  {item.Category.toUpperCase()}
                  {' / '}
                  {item.Code.toUpperCase()}
              </Text>
            </TouchableOpacity>
          );
      };

      const ItemSeperatorView = () => {
          return (
            <View
            style={{height: 0.5,
            width: '100%',
            backgroundColor: '#F0F8FF',
          }} />
          );
      };

      return (
        <View style={styles.container}>
        <Text>
            Browse OVER 3800 CATEGORIES OF BOOOOOOOKKKKKKKKKKSSS!!!
        </Text>
        <TextInput
            style={styles.textInputStyle}
            keyboardType='default'
            onChangeText={(text) => setSearchText(text)}
            value={searchText}
            placeholder="Search by Title, Author, or ISBN!"
        />
        <Button
            title='Search'
            onPress={()=>{navigation.navigate ('Search Results', {
            userInput: searchText,
            })}}
        /> 
        <TextInput
            style={styles.textInputStyle}
            keyboardType='default'
            onChangeText={(text) =>searchBisacCodes(text)}
            value={search}
            placeholder="Pick a Genre or Subject!"
        />
          
        <FlatList 
            data={filterBisacCodes}
            keyExtractor={(item, index) => index.toString()}
            ItemSeparatorComponent={ItemSeperatorView}
            renderItem={ItemView}
        />
        </View>
    );
}

//RenderFriendList needs correct API call - hard coded standin (Neo4j return) provided.
function RenderFriendList({route, navigation}) {
    // const [friendList, setFriendList] = useState([]);
    const [isLoading, setLoading] = useState(true);
    const styles = StyleSheet.create(
      {
      container: {
        flex: 1,
        backgroundColor: '#fff',
        alignItems: 'center',
        justifyContent: 'center',
      },
      buttonContainer: {
        marginTop: 20
      },
      input: {
        borderWidth: 1,
        borderColor: '#777',
        padding: 8,
        margin: 10,
        width: 200,
      },
      item: {
        marginTop: 10,
        padding: 10,
        backgroundColor: 'pink',
        fontSize: 12
      }
  });
  
  //Need to make API call using routed User information and Query database for FRIENDS_WITH relations
  //WHY ARE YOU BLOCKING THIS FETCH REQUEST MICROSOFT!!! 
  // useEffect(() => {
  //   fetch('https://raw.githubusercontent.com/KariFal56/ReadingRainbow/Neo4j-Database%2BAlgorithms/friend_list_sample')
  //       .then((response) => response.json())
  //       .then((json) => setFriendList(json.items))
  //       .catch((error) => console.error(error))
  //       .finally(() => setLoading(false));
  // }, []);
  
  const friendList =
  {
    "p2": {
  "identity": 0,
  "labels": [
        "Person"
      ],
  "properties": {
  "Portrait": "https://res.cloudinary.com/dotcpqooc/image/upload/v1610872155/iLportrait/dllyvhqkv1remxoyvqjq.jpg",
  "Email": "Yang@gmail.com",
  "HashedPassword": "9f735e0df9a1ddc702bf0a1a7b83033f9f7153a00c29de82cedadc9957289b05",
  "Profile": "FPA Fleet Admiral, Historian",
  "Name": "Yang Wen-Li"
      }
    }
  }
  
  const fName = friendList.p2.properties.Name;
  const fPortrait = friendList.p2.properties.Portrait;
  const fProfile = friendList.p2.properties.Profile;
  
  return (
    <View style = {styles.container}>
      <ScrollView>
      <Button
        title='Add Friends!'
        onPress={()=>{navigation.navigate ('Search User')}}
      />
      <TouchableOpacity onPress={()=>{navigation.navigate ('User Profile', {
          name: fName,
          portrait: fPortrait,
          profile: fProfile,
          friend: 'true',                
        })}}>
      <Text>
        <Image source={{uri: fPortrait}} style={{width: 250, height: 250}} />
        Name: {JSON.stringify(fName)}
        About Me: {JSON.stringify(fProfile)}
      </Text>            
      </TouchableOpacity> 
      </ScrollView>
    </View>
  )
}

//Generic SearchUser frontend currently awaiting middle layer API call
function SearchUsers({navigation}) {
    const [searchText, setSearchText] = useState('');
    const styles = StyleSheet.create({
      container: {
        backgroundColor: 'white',
      },
      itemStyle: {
        padding: 10,
      },
      textInputStyle: {
        height: 40,
        borderWidth: 1,
        paddingLeft: 20,
        margin: 5,
        borderColor: '#009688',
        backgroundColor: '#F0F8FF',
      },
    });
  
    return (
      <View style={styles.container}>
        <Text>
          Search our Database for Friends, Bookworms, or other Book Lovers!
        </Text>
        <TextInput
            style={styles.textInputStyle}
            keyboardType='default'
            onChangeText={(text) => setSearchText(text)}
            value={searchText}
            placeholder="Search by Username"
        />
        <Button
          title='Search'
          onPress={()=>{navigation.navigate ('User Search Results', {
            searchInput: searchText,
          })}}
        />
      </View> 
    )
}
  
function RenderUserSearchResults({route, navigation}){
  const {searchInput} = route.params;
  const [isLoading, setLoading] = useState(true);
  const [data, setData] = useState([]);
  const styles = StyleSheet.create(
    {
    container: {
      flex: 1,
      backgroundColor: '#fff',
      alignItems: 'center',
      justifyContent: 'center',
    },
    buttonContainer: {
      backgroundColor: '#7a42f4',
      padding: 10,
      margin: 15,
      height: 40,
    },
    input: {
      borderWidth: 1,
      borderColor: '#777',
      padding: 8,
      margin: 10,
      width: 200,
    },
    item: {
      marginTop: 10,
      padding: 10,
      backgroundColor: 'pink',
      fontSize: 12
    }
  });
  
  //Make Search User API call here with the routed parameter "searchInput" from SearchUser Function.
  // useEffect(() => {
  //   fetch('API CALL' + searchInput)
  //       .then((response) => response.json())
  //       .then((json) => setData(json.items))
  //       .catch((error) => console.error(error))
  //       .finally(() => setLoading(false));
  // }, [searchInput]);
  
  //For react navigation container, include the following"
  // 'User Search Results'
  // RenderUserSearchResults
  // 'Search Users'
  // SearchUsers
  
  const ItemView = ({item}) => {
    return (
      <View style = {styles.container}>
      <TouchableOpacity onPress={()=>{navigation.navigate ('Profile', {
        // name:
        // portrait:
        // profile:
        // etc:
        // friend: 'false'
      })}}>      
        <Text>Name: {item.volumeInfo.title} </Text>
        <Text>Profile: {item.volumeInfo.authors}</Text>
        {/* <Image source={{uri: }} style={{width: 128, height: 205}} /> */}
      </TouchableOpacity>
      </View>
    );
  };
  
  return (
    <View style={{ flex: 1, padding: 24}}>
        {isLoading ? <ActivityIndicator/> : (
            <FlatList
                data={data}
                keyExtractor={({ id }, index) => id}
                renderItem={ItemView}
            />
        )}
    </View>
  );
}

const Stack = createStackNavigator();

function App() {
    return (
        <NavigationContainer>
            <Stack.Navigator intialRouteName="Home">
                <Stack.Screen name="Home" component={RenderHomePage} />
                <Stack.Screen name="Profile" component={RenderProfile} />
                <Stack.Screen name="Book Information" component={BookInfo} />
                <Stack.Screen name="Selected Book Info" component={UserBookInfo} />
                <Stack.Screen name="Upload Your Portrait!" component={UploadImage} />
                <Stack.Screen name="Search for Books" component={GenreSearch} />                    
                <Stack.Screen name="Search Results" component={GoogleSearch} />  
                <Stack.Screen name="Friend List" component={RenderFriendList} />
                <Stack.Screen name="User Profile" component={RenderGenericProfile} />
                <Stack.Screen name="Search User" component={SearchUsers} />
                <Stack.Screen name="User Search Results" component={RenderUserSearchResults} />                        
            </Stack.Navigator>
        </NavigationContainer>
    )
}

export default App