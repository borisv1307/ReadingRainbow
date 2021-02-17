import React, { useEffect, useState } from 'react';
import { Image, View, Text, Button, TouchableOpacity, ScrollView, AsyncStorage, ActivityIndicator} from 'react-native';
import { globalStyles } from '../styles/global';
import { useNavigation } from '@react-navigation/native';
import { AuthContext } from '../components/context';
import { GetUserProfile } from '../api-functions/getUserProfile';
import { GetUserLibrary } from '../api-functions/getUserLibrary';
import * as SecureStore from 'expo-secure-store';

export default function Home() {
    const [ proResults, setProResults ] = useState({});
    const [ libResults, setLibResults ] = useState([]);
    const [ isLoading, setIsLoading ] = useState(true);
    const { navigate } = useNavigation();
    const { signOut } = React.useContext(AuthContext);
    async function logToken() {
        try {
            const token = await SecureStore.getItemAsync('jwt');
            console.log("At home screen token: ", token); //Remove at future time
        } catch (e) {
            console.log(e);
        }
    };

    useEffect(() => {
        AsyncStorage.getItem('username').then(user => {
            GetUserProfile(user).then(profile => setProResults(profile));
            GetUserLibrary(user).then(library => setLibResults(library));
        }).then(() => {
            if (!proResults.Profile) {
                navigate('UploadPic');
            } else {
                return;
            }
        });

    }, []);

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
