import 'react-native-gesture-handler';
import React from 'react';
import { NavigationContainer } from '@react-navigation/native';
import { createStackNavigator } from '@react-navigation/stack';
import SignIn from "./screens/sign_in";
import Home from "./screens/home";
import SignUp from "./screens/sign_up";
import ForgotPassword from "./screens/forgot_password";
import Profile from "./screens/profile";
import Library from "./screens/library";
import WishList from "./screens/wishlist";
import Book from "./screens/book";
import Search from "./screens/search";
import Settings from "./screens/settings";
import FriendList from "./screens/friend_list";
import Menu from "./screens/menu";

const Stack = createStackNavigator();

export default function App() {
    return (
        <NavigationContainer>
            <Stack.Navigator initialRouteName="SignIn">
            <Stack.Screen name="SignIn" component={SignIn} />
            <Stack.Screen name="Home" component={Home} />
            <Stack.Screen name="SignUp" component={SignUp} />
            <Stack.Screen name="ForgotPassword" component={ForgotPassword} />
            <Stack.Screen name="Profile" component={Profile} />
            <Stack.Screen name="Library" component={Library} />
           {/*} <Stack.Screen name="FrLibrary" component={FrLibraryPage} /> */}
            <Stack.Screen name="WishList" component={WishList} />
            <Stack.Screen name="Book" component={Book} />
            <Stack.Screen name="Search" component={Search} />
            {/* <Stack.Screen name="Search" component={SearchPage} /> */}
            {/* <Stack.Screen name="SearchBook" component={SearchBoPage} /> */}
            <Stack.Screen name="Settings" component={Settings} />
            <Stack.Screen name="FriendList" component={FriendList} />
            {/* <Stack.Screen name="FriWishList" component={FriWishListPage} /> */}
            <Stack.Screen name="Menu" component={Menu} />
            {/* <Stack.Screen name="Preferences" component={Preferences} /> */}
            {/* <Stack.Screen name="FrProfile" component={FrProfilePage} /> */}
      </Stack.Navigator>
    </NavigationContainer>
  );
}
