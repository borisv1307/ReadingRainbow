import React, { useEffect } from 'react';
import { Image, View, Text, Button, TouchableOpacity, ScrollView} from 'react-native';
import { globalStyles } from '../styles/global';
import { useNavigation } from '@react-navigation/native';
import { AuthContext } from '../components/context';
import * as SecureStore from 'expo-secure-store';

export default function Home() {
    const { navigate } = useNavigation();
    const { signOut } = React.useContext(AuthContext);
    async function logToken() {
        try {
            const token = await SecureStore.getItemAsync('jwt');
            console.log("At home screen token: ", token); //Remove at future time
            // if (token) {
            //     console.log("Token again: ", token);
            // }
        } catch (e) {
            console.log(e);
        }
    };
    useEffect(() => {
        logToken();
    }, [])
    return (
        <View style={globalStyles.container}>
            <ScrollView>
                <Text style={globalStyles.titleText}>Welcome, Paige!</Text>
                <Button title="Find Books" onPress={() => navigate('Search')} />
                <Button title="Friends" onPress={() => navigate('FriendList')} />
                <Button title="View My Profile" onPress={() => navigate('Profile')} />
                <Button title="Log token" onPress={() => logToken()} />
                <View>
                    <TouchableOpacity style={globalStyles.smallButton}>
                        <Text>+</Text>
                    </TouchableOpacity>
                    <TouchableOpacity style={globalStyles.smallButton}>
                        <Text>View My Library</Text>
                    </TouchableOpacity>
                </View>
                <View>
                    <Button title='Sign Out' onPress={() => {signOut()}}/>    
                    <TouchableOpacity style={globalStyles.largeButton}>
                        <Text>Books your friends recommend!</Text>
                    </TouchableOpacity>
                    <ScrollView horizontal={true}>  
                        <View style={{ flexDirection: 'row'}}>
                            <TouchableOpacity onPress={() => navigate('Book')} >
                                <Image source={require('../assets/LOTR.jpg')} style={{width: 75, height: 90}} />
                            </TouchableOpacity>
                            <TouchableOpacity onPress={() => navigate('Book')} >
                                <Image source={require('../assets/Dune.jpg')} style={{width: 75, height: 105}}/>
                            </TouchableOpacity>
                        </View>
                    </ScrollView>
                    <TouchableOpacity style={globalStyles.largeButton}>
                        <Text>Books we recommend for you!</Text>
                    </TouchableOpacity>
                </View>
            </ScrollView>
        </View>
    );
}

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
                  <Image source = {{uri:portrait}} style={{width: 360, height: 200}} />
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
                <Button
                    title='Add Books to Library or Wish List'
                    onPress={()=>{navigation.navigate ('Search for Books')}}
                />
                <Text>-----Friend List-----</Text>
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

const Stack = createStackNavigator();

function App() {
    return (
        <NavigationContainer>
            <Stack.Navigator intialRouteName="Home">
                <Stack.Screen name="Home" component={RenderHomePage} />
                <Stack.Screen name="Profile" component={RenderProfile} />
                <Stack.Screen name="Book Information" component={BookInfo} />
                <Stack.Screen name="Upload Your Portrait!" component={UploadImage} />
                <Stack.Screen name="Search for Books" component={GenreSearch} />                    
                <Stack.Screen name="Search Results" component={GoogleSearch} />                
            </Stack.Navigator>
        </NavigationContainer>
    )
}

export default App