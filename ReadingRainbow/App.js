import 'react-native-gesture-handler';
import React, { useEffect } from 'react';
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
import { View, AsyncStorage, ActivityIndicator} from 'react-native';
import { AuthContext } from './components/context';
import RootStackScreen from './screens/rootstack';
import UploadPic from './screens/upload_pic';
import * as SecureStore from 'expo-secure-store'; 

const Stack = createStackNavigator();

const App = () => {

    const initialLoginState = {
        isLoading: true,
        userName: null,
        userToken: null,
    };

    const loginReducer = (prevState, action) => {
        switch( action.type ) {
            case 'RETRIEVE_TOKEN': 
            return {
                ...prevState,
                userToken: action.token,
                isLoading: false,
            };
            case 'LOGIN': 
            return {
                ...prevState,
                userName: action.id,
                userToken: action.token,
                isLoading: false,
            };
            case 'LOGOUT': 
            return {
                ...prevState,
                userName: null,
                userToken: null,
                isLoading: false,
            };
            case 'REGISTER': 
            return {
                ...prevState,
                userName: action.id,
                userToken: action.token,
                isLoading: false,
            };
        }
    };

    const [loginState, dispatch] = React.useReducer(loginReducer, initialLoginState);

    const authContext = React.useMemo(() => ({
        signIn: async(userName, token) => {
            
            console.log(token);
            console.log(userName);
                     
            dispatch({ type: 'LOGIN', id: userName, token: token });
        },
        signOut: async() => {
            try {
              await SecureStore.deleteItemAsync('jwt');
              console.log('Signed Out');
            } catch(e) {
              console.log(e);
            }
            dispatch({ type: 'LOGOUT' });
          },
          signUp: () => {
          },
        }), []);
  
    useEffect(() => {
        SecureStore.getItemAsync('jwt').then(jwt => {
            dispatch({type: 'RETRIEVE_TOKEN', token: jwt});
        })
    }, []);

if ( loginState.isLoading ) {
    return (
        <View style={{flex:1,justifyContent:'center',alignItems:'center'}}>
            <ActivityIndicator color="black"/>
        </View>
    );
}

    return (
        <AuthContext.Provider value={authContext}>
            <NavigationContainer>
                { (loginState.userToken) ? (
                    <Stack.Navigator initialRouteName="Home" headerMode="screen">
                        <Stack.Screen name="SignIn" component={SignIn} />
                        <Stack.Screen name="Home" component={Home} />
                        <Stack.Screen name="SignUp" component={SignUp} />
                        <Stack.Screen name="ForgotPassword" component={ForgotPassword} />
                        <Stack.Screen name="Profile" component={Profile} />
                        <Stack.Screen name="Library" component={Library} />
                        <Stack.Screen name="WishList" component={WishList} />
                        <Stack.Screen name="Book" component={Book} />
                        <Stack.Screen name="Search" component={Search} />
                        <Stack.Screen name="Settings" component={Settings} />
                        <Stack.Screen name="FriendList" component={FriendList} />
                        <Stack.Screen name="Menu" component={Menu} />
                        <Stack.Screen name="UploadPic" component={UploadPic} />
                    </Stack.Navigator>
                )
                :
                    <RootStackScreen/>
                }
            </NavigationContainer>
    </AuthContext.Provider>
  );
}

export default App;
