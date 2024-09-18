import { withJsonFormsControlProps } from '@jsonforms/react';
import Divider from '@mui/material/Divider';

interface DividerControlProps {
  data: number;
  handleChange(path: string, value: number): void;
  path: string;
}

const DividerControl = ({  }: DividerControlProps) => (
  <Divider sx={{
    marginTop: '1em',
    marginBottom: '1.5em'
  }} />
);

// Fast refresh can't handle anonymous components.
const DividerControlWithJsonForms = withJsonFormsControlProps(DividerControl);
export default DividerControlWithJsonForms;
