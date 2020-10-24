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

import axios from 'axios';


const Stack = createStackNavigator();

//jest code copied from robinwieruch.de.react-testing-jest
//---------------------------------------------
import axios from 'axios';
 
export const dataReducer = (state, action) => {
  if (action.type === 'SET_ERROR') {
    return { ...state, list: [], error: true };
  }
 
  if (action.type === 'SET_LIST') {
    return { ...state, list: action.list, error: null };
  }
 
  throw new Error();
};
 
const initialData = {
  list: [],
  error: null,
};
 
const App = () => {
  const [counter, setCounter] = React.useState(0);
  const [data, dispatch] = React.useReducer(dataReducer, initialData);
 
  React.useEffect(() => {
    axios
      .get('http://hn.algolia.com/api/v1/search?query=react')
      .then(response => {
        dispatch({ type: 'SET_LIST', list: response.data.hits });
      })
      .catch(() => {
        dispatch({ type: 'SET_ERROR' });
      });
  }, []);
 
  return (
    <div>
      <h1>My Counter</h1>
      <Counter counter={counter} />
 
      <button type="button" onClick={() => setCounter(counter + 1)}>
        Increment
      </button>
 
      <button type="button" onClick={() => setCounter(counter - 1)}>
        Decrement
      </button>
 
      <h2>My Async Data</h2>
 
      {data.error && <div className="error">Error</div>}
 
      <ul>
        {data.list.map(item => (
          <li key={item.objectID}>{item.title}</li>
        ))}
      </ul>
    </div>
  );
};
 
export const Counter = ({ counter }) => (
  <div>
    <p>{counter}</p>
  </div>
);
 
export default App;
//----------------------------------------------
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
            <Button label ="click me please"></Button>
      </Stack.Navigator>
    </NavigationContainer>
  );
}
