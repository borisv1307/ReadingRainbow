import React from 'react';
import { act } from 'react-test-renderer';
import Book from '../book';
import { NavigationContainer } from "@react-navigation/native"
import { createStackNavigator } from '@react-navigation/stack';
import { render } from '@testing-library/react-native';
// import MockedNavigator from '../mocked_navigator';

const Stack = createStackNavigator();

test('two plus three is five', () => {
    expect(2 + 3).toBe(5);
});

describe('Book renders correctly', () => {
    it ('should match snapshot', async () => {
        const result = render(
            <NavigationContainer>
                <Stack.Screen name="Book" component={Book} />
            </NavigationContainer>
        );
        await act(async () => { expect(result).toMatchSnapshot(); })
    })
})

// it('Book renders correctly', () => {
//     const {toJSON} = render(
//       <MockedNavigator component={Book} />
//     );
//     expect(toJSON()).toMatchSnapshot();
// });

// it('Book renders correctly', () => {
//     let tree;

//     act(() => {
//         tree = create(
//             <MockedNavigator component={Book} />
//         );
//     });
//     expect(tree).toMatchSnapshot();
// });

