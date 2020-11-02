import react from 'react';
import React, {useEffect, useState } from 'react';
import {SafeAreaView, Text, StyleSheet, View, TextInput, Button, Image, FlatList, TouchableOpacity, ScrollView} from 'react-native';

const App = () => {
    const [search, setSearch] = useState('');
    const [filterBisacCodes, setFilterBisacCodes] = useState([]);
    const [bisacCodes, setBisacCodes] = useState([]);
        
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
              <TouchableOpacity>
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
    };

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
    
export default App;