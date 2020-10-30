import React from 'react';
import renderer from 'react-test-renderer';
import SignIn from '../sign_in';

describe('Sign In', () => {
  it('renders correctly', () => {
    const tree = renderer.create(
      <SignIn/>
    ).toJSON();
    expect(tree).toMatchSnapshot();
  });
});