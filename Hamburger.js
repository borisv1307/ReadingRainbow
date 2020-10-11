import {createAppContainer} from 'react-navigation';
import {createDrawerNavigator, DrawerItems} from 'react-navigation-drawer';
import {Header} from 'react-native-elements';
import {View, Text, TouchableOpacity, Image} from 'react-native'
import { render } from 'react-dom';

import { createDrawerNavigator } from '@react-navigation/drawer';
import { setStatusBarNetworkActivityIndicatorVisible } from 'expo-status-bar';

const Drawer = createDrawerNavigator();

function MyDrawer() {
  return (
    <Drawer.Navigator>
      <Drawer.Screen name="Find Books" component={SearchBoPage} />
      <Drawer.Screen name="My Friends" component={FriendListPage} />
      <Drawer.Screen name="Browse Books" component={BrowseBooksPage} />
      <Drawer.Screen name="Wish List" component={WishListPage} />
      <Drawer.Screen name="My Libary" component={MyLibraryPage} />
      <Drawer.Screen name="My Profile" component={ProfilePage} />
      <Drawer.Screen name="Settings" component={SettingsPage} />
      <Drawer.Screen name="Log Out" component={SignInPage} />
    </Drawer.Navigator>
  );
}

export default function App() {
    return (
      <NavigationContainer>
        <MyDrawer />
      </NavigationContainer>
    );
  }

"Scan Books"
"Browse Books"
"WishList"
"My Library"
"My Profile"
"Settings"
"Log Out"

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
                <Stack.Screen name="Search" component={SearchPage} />
                <Stack.Screen name="SearchBook" component={SearchBoPage} />
                <Stack.Screen name="Settings" component={SettingsPage} />
                <Stack.Screen name="Results" component={ResultsPage} />
                <Stack.Screen name="FriendList" component={FriendListPage} />
                <Stack.Screen name="FriWishList" component={FriWishListPage} />
            </Stack.Navigator>
        </NavigationContainer>
    )
}

const MenuHamStack = (props) => {
    render() {
        return (
            <View style={{}}>
                <DrawerItems{...props}/>
            </View>
        );
    }
};

const Drawer = createDrawerNavigator(
    {
        Profile: {
            screen: Profilepage,
            navigationOptions: {
                title: 'Profile'
            }
        },
        Settings: {
            screen: SettingsPage,
            navigationOptions: {
                title: 'Settings'
            }
        },
        Request: {
            screen: RequestPage,
            navigationOptions: {
                title: 'Requests'
            }
        },
        WishList: {
            screen: WishListPage,
            navigationOptions: {
                title: 'WishList'
            }
        },
        Reading: {
            screen: ReadingPage,
            navigationOptions: {
                title: 'Currently Reading'
            }
        },
        SignOut: {
            screen: SignInPage,
            navigationOptions: {
                title: 'Sign Out'
            }
        }
        {
            drawerPosition: 'left',
            contentComponent: MenuHamStack,
            drawerOpenRoute: 'DrawerOpen',
            drawerCloseRoute: 'DrawerClose',
            drawerToggleRoute: 'DrawerToggle',
            drawerWidth: (width / 3) * 2,
        }  
    }
);

const App = createAppContainer(Drawer);

export default App;

// 
{/* <Header  */}
    // style={styles.menu}
    // <TouchableOpacity onPress={() =>this.props.navigation.openDrawer()}>
        // <Image source={require ('../assets/Hamburger.png')} style={{width: 50, height: 50}} />
    // </TouchableOpacity>
// />

class HomePage extends React.Component{
    render() {
      return (
        <View style={styles.container}>
            <Header 
                style={styles.menu}
                leftComponent = {<Image source={require ('../assets/Hamburger.png')} style={{width: 50, height: 50}} onPress={() =>this.props.navigation.openDrawer()}/>}
                rightComponent = {<Image source={require ('../assets/ProfileIcon.png')} style={{width: 50, height: 50}} onPress={() =>this.props.navigation.openDrawer()}/>}
            />
          <Text>Welcome User_Name!</Text>
          <Button
              title= 'Find Books'
              onPress={() =>this.props.navigation.navigate('Results')}
            />
            <Button
              title= 'Friends'
              onPress={() =>this.props.navigation.navigate('FriendList')}
            />
            <Button
              title= 'Browse Books'
              onPress={() =>this.props.navigation.navigate('SearchBook')}
            />
            <Button
              title= 'Scan Barcode'
              onPress={() =>this.props.navigation.navigate('ScanBook')}
            />
            <Text>'Books your Friends Recommend!'</Text>
            <Image source={require('../assets/GOTs.jpeg')} />
            <Text>'Books we recommend!'</Text>
            <Image source={require('../assets/LOTR.jpeg')} />
        </View>
      )
    }
  }


const styles = StyleSheet.create({
    menu: {

    }
})

onMenuClick(){
    this.props.toggleDrawer();
}