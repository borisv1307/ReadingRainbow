import { GetBooks } from '../getbooks';

beforeEach(() => {
    fetch.resetMocks();
});
  
test('returns result if array', () => {
    fetch.mockResponseOnce(JSON.stringify([{ id: 1 }]));
});