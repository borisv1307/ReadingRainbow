import { StyleSheet } from 'react-native';
import colors from './colors.js';
export const globalStyles = StyleSheet.create({
  avatar: {
    height: 100,
    width: 100,
  },
  box: {
    backgroundColor: colors.LIGHT_GREEN,
    flex: 1,
    padding: 6,
    margin: 6,
  },
  bookImage: {
    
  },
  buttonText: {
    fontSize: 16,
    flexWrap: 'wrap',
  },
  container: {
    flex: 1,
    padding: 60,
    justifyContent: 'space-between',
  },
  icon: {
    width: 30,
    height: 30,
    margin: 10,
  },
  input: {
    padding: 8,
    margin: 10,
    flexDirection: 'row',
    
  },
  item: {
    marginTop: 10,
    padding: 10,
    backgroundColor: colors.RED,
    fontSize: 12
  },
  largeButton:{
    fontSize: 200,
    fontWeight: 'bold',
    color: '#333',
    padding: 60,
    backgroundColor: "yellow",
    borderColor: "black",
    margin: 10,
    justifyContent: "space-around"
  },
  profileInfo: {
    
  },
  smallButton: {
    padding: 30,
    backgroundColor: "beige",
    margin: 10,
    flexWrap: "wrap",
    justifyContent: "space-between",
    alignContent: "space-between",
  },
  titleText: {
    fontSize: 18,
    fontWeight: "bold",
    color: "#333",
    textAlign: "center",
  },
  thumbnail: {
    height: 193,
    width: 128,
    margin: 4,
  },
  warningButton:{
    padding: 40,
    margin: 10,
    flexWrap: "wrap",
    justifyContent: "space-between",
    alignContent: "space-between",
  },
});