import { StyleSheet } from 'react-native';

export const globalStyles = StyleSheet.create({
  titleText: {
    fontSize: 18,
    fontWeight: "bold",
    color: "#333",
    textAlign: "center",
  },
  container: {
    flex: 1,
    padding: 60,
  },
  input: {
    padding: 8,
    margin: 10,
    backgroundColor: "beige",
  },
  bookImage: {
    
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
  warningButton:{
    padding: 40,
    backgroundColor: "red",
    margin: 10,
    flexWrap: "wrap",
    justifyContent: "space-between",
    alignContent: "space-between",
  },
  buttonText: {
    fontSize: 16
  },
  box: {
    backgroundColor: "beige",
    flex: 1,
    padding: 6,
    margin: 6,
  },
  avatar: {
    height: 100,
    width: 100,
  },
  thumbnail: {
    height: 193,
    width: 128,
    margin: 4,
  },
  item: {
    marginTop: 10,
    padding: 10,
    backgroundColor: 'pink',
    fontSize: 12
  }
});