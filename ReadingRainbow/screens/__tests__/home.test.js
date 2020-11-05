const { default: Results } = require("../results");

test('two plus three is five', () => {
    expect(2 + 3).toBe(5);
});

import React from 'react';
import renderer from 'react-test-renderer';
import { StyleSheet, View, Text } from 'react-native';

it('results renders correctly', () => {
    const tree = renderer.create(<Results />).toJSON();
    expect(tree).toMatchSnapshot();
});