import React from 'react';
import * as renderer from 'react-test-renderer';
import { StyleSheet, Image, View, Text, Button, TouchableOpacity, ScrollView} from 'react-native';
import { globalStyles } from '../../styles/global';
import { useNavigation } from '@react-navigation/native';
import MockedNavigator from '../mocked_navigator';
import Results from "../results";

test('two plus three is five', () => {
    expect(2 + 3).toBe(5);
});

it('Results renders correctly', () => {
    const tree = renderer.create(<Results />).toJSON();
    expect(tree).toMatchSnapshot();
});

// it('Home renders correctly', () => {
//     const {toJSON} = render(
//       <MockedNavigator component={Home} />
//     );
//     expect(toJSON()).toMatchSnapshot();
// });

// it('Results renders correctly', () => {
//     let tree;

//     act(() => {
//         tree = create(
//             <MockedNavigator component={Results} />
//         );
//     });
//     expect(tree).toMatchSnapshot();
// });