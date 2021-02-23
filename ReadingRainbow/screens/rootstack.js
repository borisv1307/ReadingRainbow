import React from 'react';
import { createStackNavigator } from '@react-navigation/stack';
import SignIn from './sign_in';
import SignUp from './sign_up';
import ForgotPassword from './forgot_password';

const RootStack = createStackNavigator();

const RootStackScreen = () => (
    <RootStack.Navigator headerMode="screen">
        <RootStack.Screen name="SignInScreen" component={SignIn}/>
        <RootStack.Screen name="SignUpScreen" component={SignUp}/>
        <RootStack.Screen name="ForgotPassword" component={ForgotPassword}/>
    </RootStack.Navigator>
);

export default RootStackScreen;