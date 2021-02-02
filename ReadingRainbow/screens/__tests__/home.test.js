import React from 'react';
import { act } from 'react-test-renderer';
import Home from '../home';
import { NavigationContainer } from "@react-navigation/native"
import { createStackNavigator } from '@react-navigation/stack';
import { render } from '@testing-library/react-native';

const Stack = createStackNavigator();

test('two plus three is five', () => {
    expect(2 + 3).toBe(5);
});

describe('Home renders correctly', () => {
    it ('should match snapshot', async () => {
        const result = render(
            <NavigationContainer>
                <Stack.Screen name="Home" component={Home} />
            </NavigationContainer>
        );
        await act(async () => { expect(result).toMatchSnapshot(); })
    })
})

// it('Home renders correctly', () => {
//     const {toJSON} = render(
//       <MockedNavigator component={Home} />
//     );
//     expect(toJSON()).toMatchSnapshot();
// });

// it('Home renders correctly', () => {
//     let tree;

//     act(() => {
//         tree = create(
//             <MockedNavigator component={Home} />
//         );
//     });
//     // const tree = renderer.create(<Home />).toJSON();
//     expect(tree).toMatchSnapshot();
// });