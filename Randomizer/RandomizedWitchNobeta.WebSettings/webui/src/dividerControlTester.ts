import { rankWith, scopeEndsWith, uiTypeIs } from '@jsonforms/core';

export default rankWith(
  3, //increase rank as needed
  // scopeEndsWith('Divider'),
  uiTypeIs('Divider')
);
