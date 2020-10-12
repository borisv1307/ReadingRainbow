import * as React from 'react';
import { StyleSheet, Text, View, TextInput, Button, Image, FlatList, TouchableOpacity, ScrollView, Animated } from 'react-native';
import { DrawerActions, NavigationContainer } from '@react-navigation/native';
import { createStackNavigator } from '@react-navigation/stack';
import { DrawerContentScrollView, DrawerItemList, DrawerItem, createDrawerNavigator } from '@react-navigation/drawer';
import { Header } from 'react-native-elements';

class SignInPage extends React.Component {
    render() {
      return (
        <View>
          <Text>Welcome to Name_Proj, an App for Literature Nerds to share Libraries, Browse Books, Find Friends, Recommend Books and More!</Text>
          <View style={styles.buttonContainer}>
            <Button
              title= 'Google/Apple Sign In'
              onPress={() =>this.props.navigation.navigate('Home')}
            />
            <Button
              title= 'Sign In'
              onPress={() =>this.props.navigation.navigate('Home')}
            />
            <Button
              title= 'Create Account'
              onPress={() =>this.props.navigation.navigate('CreateAcc')}
            />
            <Button
              title= 'Forgot Password?'
              onPress={() =>this.props.navigation.navigate('ForgotP')}
            />
        </View>
        </View>
      );
    }
  }

class CreatePage extends React.Component{
    render() {
      return (
        <View style={styles.container}>
          <Text>Enter Username:</Text>
          <TextInput style={styles.input}
          keyboardType='default'
          placeholder='e.g. user@gmail.com'
          onChangeText={(val) => setName(val)} />
  
          <Text>Enter Password:</Text>
          <TextInput style={styles.input} 
          keyboardType='default'
          placeholder='Try a Passphrase!'
          onChangeText={(val) => setPassword(val)} />
          
          <Button
            title= 'Sign In'
            onPress={() =>this.props.navigation.navigate('InitialPreferences')}
          />
        </View>
      )
    }
  }

class ForgotPage extends React.Component{
  render() {
    return (
      <View style={styles.container}>
        <Text>Enter Account Email:</Text>
        <TextInput style={styles.input}
        keyboardType='default'
        placeholder='e.g. user@gmail.com'
        onChangeText={(val) => setEmail(val)} />
        <View style={styles.buttonContainer}>
          <Button
            title='Submit!'
            onPress = {() => this.props.navigation.navigate('SignIn')}
          />
        </View>
      </View>
    )
  }
}

class HomePage extends React.Component{
  render() {
    return (
      <View style={styles.container}>
        <Header
          style={styles.menu}
          leftComponent = {<Image source={require ('Hamburger.png')} style={{width: 50, height: 50}} onPress={() =>this.props.navigation.navigate('Menu')}/>}
        />
        <Button
            title='Menu'
            onPress={() => this.props.navigation.navigate('Menu')}
        />
          <Text>'Welcome User_Name'</Text>
          <Text>'Books your Friends Recommend!'</Text>
          <ScrollView horizontal={true}>  
            <TouchableOpacity onPress={()=>this.props.navigation.navigate('BookInfo')}>
            <View>
              <View style={{ flexDirection: 'row'}}>
              <Image source={require('GOTs.jpeg')} style={{width: 75, height: 75}} />
              <Image source={require('LOTR.jpg')} style={{width: 75, height: 90}} />
              <Image source={require('Dune.jpg')} style={{width: 75, height: 105}}/>
              </View>
            </View>
            </TouchableOpacity>
          </ScrollView>
          <Text>'Books we recommend!'</Text>
          <ScrollView horizontal={true}>
            <TouchableOpacity onPress={()=>this.props.navigation.navigate('BookInfo')}>
            <View>
              <View style={{ flexDirection:'row'}}>
              <Image source={require('LOTR.jpg')} style={{width: 75, height: 90}} />
              <Image source={require('Dune.jpg')} style={{width: 75, height: 105}}/>
              <Image source={require('GOTs.jpeg')} style={{width: 75, height: 75}} />
              </View>
            </View>
            </TouchableOpacity>
          </ScrollView>
          <Text>'Books Your Friends are Reading!'</Text>
          <ScrollView horizontal={true}>
            <TouchableOpacity onPress={()=>this.props.navigation.navigate('BookInfo')}>
            <View>
              <View style={{ flexDirection:'row'}}>
              <Image source={require('Dune.jpg')} style={{width: 75, height: 105}}/>
              <Image source={require('LOTR.jpg')} style={{width: 75, height: 90}} />
              <Image source={require('GOTs.jpeg')} style={{width: 75, height: 75}} />
              </View>
            </View>
            </TouchableOpacity>
          </ScrollView>
      </View>
    )
  }
}

class MyLibraryPage extends React.Component{
  render() {
    return (
      <View style={styles.container}>
      <ScrollView>
        <Text>My Library</Text>
        <Button
          title='Find/Browse Books'
          onPress={() =>this.props.navigation.navigate('BrowseBooks')}
        />
        <TouchableOpacity onPress={()=>this.props.navigation.navigate('BookInfo')}>
          <Image source={require('GOTs.jpeg')} style={{width: 50, height: 50}} />
          <Image source={require('LOTR.jpg')} style={{width: 50, height: 60}} />
          <Image source={require('Dune.jpg')} style={{width: 50, height: 70}} />
        </TouchableOpacity>
      </ScrollView>
      </View>
    )
  }
}

class WishListPage extends React.Component{
  render() {
    return (
      <View style={styles.container}>
      <ScrollView>
      <Text>My WishList</Text>
        <Button
          title='Find/Browse Books'
          onPress={() =>this.props.navigation.navigate('BrowseBooks')}
        />
        <TouchableOpacity onPress={()=>this.props.navigation.navigate('BookInfo')}>
          <Image source={require('GOTs.jpeg')} style={{width: 50, height: 50}} />
          <Image source={require('LOTR.jpg')} style={{width: 50, height: 60}} />
          <Image source={require('Dune.jpg')} style={{width: 50, height: 70}} />
        </TouchableOpacity>
      </ScrollView>
      </View>
    )
  }
}

class BookPage extends React.Component{
  render() {
    return (
      <View style={styles.container}>
        <Image source={require('Dune.jpg')}></Image>

        <Text>Book Information from Google API</Text>
        <Text>E.G. Frank Herbert's Dune (1965), Science Fiction, etc.</Text>    
        <Button
          title='WishList'
          onPress={() =>this.props.navigation.navigate('WishList')}
        />
        <Button
          title='Add to Library'
          onPress={() =>this.props.navigation.navigate('MyLibrary')}
        />    
      </View>
    )
  }
}

class ProfilePage extends React.Component{
    render() {
      return (
        <View style={styles.container}>
        <ScrollView>
          <Text>My Profile</Text>
          <Text>About Me:</Text>
          <Button
            title= 'Edit Profile Information'
          />
          <Button
            title= 'WishList'
            onPress={() =>this.props.navigation.navigate('WishList')}
          />
          <ScrollView horizontal={true}>  
            <TouchableOpacity onPress={()=>this.props.navigation.navigate('BookInfo')}>
            <View>
              <View style={{ flexDirection: 'row'}}>
              <Image source={require('GOTs.jpeg')} style={{width: 75, height: 75}} />
              <Image source={require('LOTR.jpg')} style={{width: 75, height: 90}} />
              <Image source={require('Dune.jpg')} style={{width: 75, height: 105}}/>
              </View>
            </View>
            </TouchableOpacity>
          </ScrollView>
          
          <Button 
            title= 'Library'
            onPress={() =>this.props.navigation.navigate('MyLibrary')}
          />
          <ScrollView horizontal={true}>  
            <TouchableOpacity onPress={()=>this.props.navigation.navigate('BookInfo')}>
            <View>
              <View style={{ flexDirection: 'row'}}>
              <Image source={require('Dune.jpg')} style={{width: 75, height: 105}} />
              <Image source={require('GOTs.jpeg')} style={{width: 75, height: 75}} />
              <Image source={require('LOTR.jpg')} style={{width: 75, height: 90}} />
              </View>
            </View>
            </TouchableOpacity>
          </ScrollView>
  
          <Text>'Currently Reading'</Text>
          <ScrollView horizontal={true}>  
            <TouchableOpacity onPress={()=>this.props.navigation.navigate('BookInfo')}>
            <View>
              <View style={{ flexDirection: 'row'}}>
              <Image source={require('Dune.jpg')} style={{width: 75, height: 105}} />
              <Image source={require('GOTs.jpeg')} style={{width: 75, height: 75}} />
              <Image source={require('LOTR.jpg')} style={{width: 75, height: 90}} />
              </View>
            </View>
            </TouchableOpacity>
          </ScrollView>
          <Text>Friends</Text>
          <ScrollView>
          <TouchableOpacity  onPress={() =>this.props.navigation.navigate('FrProfile')}>
          <FlatList
            data={[
                {key: 'Yang Wen-Li'},
                {key: 'Reinhard von Lohengramm'},
                {key: 'Oskar von Reuenthal'},
                {key: 'Julian Minci'},
            ]}
            renderItem={({item}) => <Text style={styles.item}>{item.key}</Text>}
            />
            </TouchableOpacity>
            </ScrollView>
        </ScrollView>
        </View>
      )
    }
  }

  class FrProfilePage extends React.Component{
    render() {
      return (
        <View style={styles.container}>
        <ScrollView>
          <Text>Friend_1's Profile</Text>
          <Text>About Me:</Text>
          <Button
            title= 'WishList'
            onPress={() =>this.props.navigation.navigate('FriWishList')}
          />
          <ScrollView horizontal={true}>  
            <TouchableOpacity onPress={()=>this.props.navigation.navigate('BookInfo')}>
            <View>
              <View style={{ flexDirection: 'row'}}>
              <Image source={require('GOTs.jpeg')} style={{width: 75, height: 75}} />
              <Image source={require('LOTR.jpg')} style={{width: 75, height: 90}} />
              <Image source={require('Dune.jpg')} style={{width: 75, height: 105}}/>
              </View>
            </View>
            </TouchableOpacity>
          </ScrollView>
          
          <Button 
            title= 'Library'
            onPress={() =>this.props.navigation.navigate('FrLibrary')}
          />
          <ScrollView horizontal={true}>  
            <TouchableOpacity onPress={()=>this.props.navigation.navigate('BookInfo')}>
            <View>
              <View style={{ flexDirection: 'row'}}>
              <Image source={require('Dune.jpg')} style={{width: 75, height: 105}} />
              <Image source={require('GOTs.jpeg')} style={{width: 75, height: 75}} />
              <Image source={require('LOTR.jpg')} style={{width: 75, height: 90}} />
              </View>
            </View>
            </TouchableOpacity>
          </ScrollView>
  
          <Text>'Currently Reading'</Text>
          <ScrollView horizontal={true}>  
            <TouchableOpacity onPress={()=>this.props.navigation.navigate('BookInfo')}>
            <View>
              <View style={{ flexDirection: 'row'}}>
              <Image source={require('Dune.jpg')} style={{width: 75, height: 105}} />
              <Image source={require('GOTs.jpeg')} style={{width: 75, height: 75}} />
              <Image source={require('LOTR.jpg')} style={{width: 75, height: 90}} />
              </View>
            </View>
            </TouchableOpacity>
          </ScrollView>
          <ScrollView>
            <Text>Friends</Text>
          <TouchableOpacity  onPress={() =>this.props.navigation.navigate('FrProfile')}>
          <FlatList
            data={[
                {key: 'Yang Wen-Li'},
                {key: 'Reinhard von Lohengramm'},
                {key: 'Oskar von Reuenthal'},
                {key: 'Julian Minci'},
            ]}
            renderItem={({item}) => <Text style={styles.item}>{item.key}</Text>}
            />
            </TouchableOpacity>
            </ScrollView>
        </ScrollView>
        </View>
      )
    }
  }

class BrowseBooksPage extends React.Component{
  render() {
    return (
      <View style={styles.container}>
        <Text>Browse Books by:</Text>
        
        <Text>Enter Title:</Text>
        <TextInput style={styles.input}
        keyboardType='default'
        placeholder='e.g. Lord of the Rings'
        onChangeText={(val) => setName(val)} />

        <Text>Enter Author:</Text>
        <TextInput style={styles.input}
        keyboardType='default'
        placeholder='e.g. J. R. R. Tolkien'
        onChangeText={(val) => setName(val)} />

        <Text>Enter ISBN:</Text>
        <TextInput style={styles.input}
        keyboardType='default'
        placeholder='e.g. 978-3-16-148410-0'
        onChangeText={(val) => setName(val)} />

        <Text>Enter Genre:</Text>
        <TextInput style={styles.input}
        keyboardType='default'
        placeholder='e.g. High Fantasy'
        onChangeText={(val) => setName(val)} />
      <Button
        title='Search!'
      />
      </View>
    )
  }
}

class SettingsPage extends React.Component{
  render() {
    return (
      <View style={styles.container}>
        <Button
          title='Privacy'
        />
        <Button
          title='Change Password'
        />
        <Button
          title='Change Recommendations'
        />
        <Button
          title='Delete Account'
        />
      </View>
    )
  }
}

class MenuPage extends React.Component{
    render() {
      return (
        <View style={styles.container}>
        <Button
          title='My Profile'
          onPress={() =>this.props.navigation.navigate('Profile')}
        />
        <Button
          title='My Library'
          onPress={() =>this.props.navigation.navigate('MyLibrary')}
        />
        <Button
          title='Wishlist'
          onPress={() =>this.props.navigation.navigate('WishList')}
        />
        <Button
          title='Browse Books'
          onPress={() =>this.props.navigation.navigate('BrowseBooks')}
        />
        <Button
          title='Settings'
          onPress={() =>this.props.navigation.navigate('Settings')}
        />
        <Button
          title='Friend List'
          onPress={() =>this.props.navigation.navigate('FriendList')}
        />
        </View>
      )
    }
  }

class FrLibraryPage extends React.Component{
    render() {
      return (
        <View style={styles.container}>
        <ScrollView>
          <Text>Friend_1's Library</Text>
          <TouchableOpacity onPress={()=>this.props.navigation.navigate('BookInfo')}>
            <Image source={require('GOTs.jpeg')} style={{width: 50, height: 50}} />
            <Image source={require('LOTR.jpg')} style={{width: 50, height: 60}} />
            <Image source={require('Dune.jpg')} style={{width: 50, height: 70}} />
          </TouchableOpacity>
        </ScrollView>
        </View>
      )
    }
  }

class FriendListPage extends React.Component{
    render() {
      return (
        <View style={styles.container}>
          <Text>Friends List</Text>
          <Text>Enter Username or Email:</Text>
          <TextInput style={styles.input}
            keyboardType='default'
            placeholder='Enter Username'
            onChangeText={(val) => setName(val)} />
          <Button
            title='Search!'
          />
          <ScrollView>
          <TouchableOpacity  onPress={() =>this.props.navigation.navigate('FrProfile')}>
          <FlatList
            data={[
                {key: 'Yang Wen-Li'},
                {key: 'Reinhard von Lohengramm'},
                {key: 'Oskar von Reuenthal'},
                {key: 'Julian Minci'},
            ]}
            renderItem={({item}) => <Text style={styles.item}>{item.key}</Text>}
            />
            </TouchableOpacity>
            </ScrollView>
        </View>
      )
    }
  }

class FriWishListPage extends React.Component{
    render() {
      return (
        <View style={styles.container}>
        <ScrollView>
        <Text>Friend_1's WishList</Text>
          <TouchableOpacity onPress={()=>this.props.navigation.navigate('BookInfo')}>
            <Image source={require('GOTs.jpeg')} style={{width: 50, height: 50}} />
            <Image source={require('LOTR.jpg')} style={{width: 50, height: 60}} />
            <Image source={require('Dune.jpg')} style={{width: 50, height: 70}} />
          </TouchableOpacity>
        </ScrollView>
        </View>
      )
    }
  }

class InitPreferencesPage extends React.Component{
    render() {
        return (
            <View style={styles.container}>
                <Text>Create Your Profile! Your Profile is set to Private by default, only friends you accepted can see your profile.</Text>
                <TextInput style={styles.input}
                keyboardType='default'
                placeholder='Profile Information'/>
                <TouchableOpacity>
                    <Text>Upload a Profile Image!</Text>
                </TouchableOpacity>
                <Text>Tell us about your book preferences!</Text>
                <TextInput style={styles.input}
                keyboardType='default'
                placeholder='Favorite Author?'/>
                <TextInput style={styles.input}
                keyboardType='default'
                placeholder='Favorite Book?'/>
                <Text>Please Select Genres You Like!</Text>
                <Text>The genres available will ultimately depend on the categories listed under the Google Library API. e.g.</Text>
                <TouchableOpacity>
                  <Text>Science Fiction</Text>
                  <Text>High Fantasy</Text>
                  <Text>Romance</Text>
                </TouchableOpacity>
                <Button
                    title='Continue to Homepage!'
                    onPress={() =>this.props.navigation.navigate('Home')}
                />
            </View>
        )
    }
}

// const [friend, setFriend] = useState([
//    {friend: 'Yang Wen-Li', key: '1'},
//      {friend: 'Reinhard von Lohengramm', key: '2'},
//     {friend: 'Oskar von Reuenthal', key: '3'},
//     {friend: 'Julian Minci', key: '4'}
//   ])

const friend = [
  {
    id: '1',
    friend: 'Yang Wen-Li',
  },
  {
    id: '2',
    friend: 'Reinhard von Lohengramm',
  },
  {
    id: '3',
    friend: 'Oskar von Reuenthal', 
  },
  {
    id: '4',
    friend: 'Julian Minci',
  },
];

const Stack = createStackNavigator();

function App() {
    return (
        <NavigationContainer>
            <Stack.Navigator intialRouteName="SignIn">
                <Stack.Screen name="SignIn" component={SignInPage} />
                <Stack.Screen name="Home" component={HomePage} />
                <Stack.Screen name="CreateAcc" component={CreatePage} />
                <Stack.Screen name="ForgotP" component={ForgotPage} />
                <Stack.Screen name="Profile" component={ProfilePage} />
                <Stack.Screen name="MyLibrary" component={MyLibraryPage} />
                <Stack.Screen name="FrLibrary" component={FrLibraryPage} />
                <Stack.Screen name="WishList" component={WishListPage} />
                <Stack.Screen name="BookInfo" component={BookPage} />
                <Stack.Screen name="BrowseBooks" component={BrowseBooksPage} />
                {/* <Stack.Screen name="Search" component={SearchPage} /> */}
                {/* <Stack.Screen name="SearchBook" component={SearchBoPage} /> */}
                <Stack.Screen name="Settings" component={SettingsPage} />
                {/* <Stack.Screen name="Results" component={ResultsPage} /> */}
                <Stack.Screen name="FriendList" component={FriendListPage} />
                <Stack.Screen name="FriWishList" component={FriWishListPage} />
                <Stack.Screen name="Menu" component={MenuPage} />
                <Stack.Screen name="InitialPreferences" component={InitPreferencesPage} />
                <Stack.Screen name="FrProfile" component={FrProfilePage} /> 
            </Stack.Navigator>
        </NavigationContainer>
    )
}
  
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
  
export default App;
