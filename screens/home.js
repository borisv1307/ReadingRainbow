import React from 'react';
import { Image, View, Text, Button, TouchableOpacity, ScrollView} from 'react-native';
import { globalStyles } from '../styles/global';
import { useNavigation } from '@react-navigation/native';

export default function Home() {
    const { navigate } = useNavigation();
    return (
        <View style={globalStyles.container}>
            <ScrollView>
                <Text style={globalStyles.titleText}>Welcome, Paige!</Text>
                <Button title="Find Books" onPress={() => navigate('Search')} />
                <Button title="Friends" onPress={() => navigate('FriendList')} />
                <Button title="View My Profile" onPress={() => navigate('Profile')} />
                <View>
                    <TouchableOpacity style={globalStyles.smallButton}>
                        <Text>+</Text>
                    </TouchableOpacity>
                    <TouchableOpacity style={globalStyles.smallButton}>
                        <Text>View My Library</Text>
                    </TouchableOpacity>
                </View>
                <View>    
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
