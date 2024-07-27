import { FC, useMemo, useState } from 'react';
import { JsonForms } from '@jsonforms/react';
import Grid from '@mui/material/Grid';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';
import {
  materialCells,
  materialRenderers,
} from '@jsonforms/material-renderers';

import RatingControl from './RatingControl';
import ratingControlTester from '../ratingControlTester';
import DividerControl from './DividerControl';
import dividerControlTester from '../dividerControlTester';
import ValueSliderControl from './ValueSliderControl';
import { valueSliderControlTester } from './ValueSliderControl';

import schema from '../schema.json';
import uischema from '../uischema.json';

const classes = {
  container: {
    padding: '1em',
    width: '100%',
  },
  title: {
    textAlign: 'center',
    padding: '0.25em',
  },
  dataContent: {
    display: 'flex',
    justifyContent: 'center',
    borderRadius: '0.25em',
    backgroundColor: '#cecece',
    marginBottom: '1rem',
  },
  resetButton: {
    margin: 'auto !important',
    display: 'block !important',
  },
  demoform: {
    margin: 'auto',
    padding: '1rem',
  },
};

const initialData = {
  Seed: Math.floor(Math.random() * 2147483647),

  Difficulty: 'Advanced',

  StartLevel: 'Random',
  ShuffleExits: true,

  GameTips: true,

  NoArcane: false,
  MagicUpgrade: 'Vanilla',
  BookAmount: 1,

  BossHunt: false,
  MagicMaster: false,

  TrialKeys: false,
  TrialKeysAmount: 5,

  OneHitKO: false,
  DoubleDamage: false,
  HalfDamage: false,

  ChestSoulCount: 250,

  StartSoulsModifier: 1.0,

  ItemWeightSouls: 3,
  ItemWeightHP: 1,
  ItemWeightMP: 1,
  ItemWeightDefense: 1,
  ItemWeightHoly: 1,
  ItemWeightArcane: 2
};

const renderers = [
  ...materialRenderers,
  //register custom renderers
  { tester: ratingControlTester, renderer: RatingControl },
  { tester: dividerControlTester, renderer: DividerControl },
  { tester: valueSliderControlTester, renderer: ValueSliderControl }
];

export const JsonFormsDemo: FC = () => {
  const [data, setData] = useState<object>(initialData);
  const stringifiedData = useMemo(() => JSON.stringify(data, null, 2), [data]);

  const clearData = () => {
    setData({});
  };

  return (
    <Grid
      container
      justifyContent={'center'}
      spacing={1}
      style={classes.container}>
      {/* <Grid item sm={6}>
        <Typography variant={'h4'}>Bound data</Typography>
        <div style={classes.dataContent}>
          <pre id="boundData">{stringifiedData}</pre>
        </div>
        <Button
          style={classes.resetButton}
          onClick={clearData}
          color="primary"
          variant="contained"
          data-testid="clear-data">
          Clear data
        </Button>
      </Grid> */}
      <Grid item xs={12} md={10} lg={8} xl={6} >
        <Typography variant={'h4'}>Settings</Typography>
        <div style={classes.demoform}>
          <JsonForms
            schema={schema}
            uischema={uischema}
            data={data}
            renderers={renderers}
            cells={materialCells}
            onChange={async event => { setData(event.data); if (!event.errors || event.errors.length == 0) {
                await fetch("/settings", {
                method: 'POST',
                headers: {
                  'Accept': 'application/json',
                  'Content-Type': 'application/json'
                },
                body: JSON.stringify(event.data, null, 4)
              })
            }}}
          />
        </div>
      </Grid>
    </Grid>
  );
};
