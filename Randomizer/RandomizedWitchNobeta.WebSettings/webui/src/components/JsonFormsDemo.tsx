import { FC, useEffect, useMemo, useState } from 'react';
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

  exportButton: {
    display: 'block !important',
    margin: '1em !important',
    marginRight: '.5em !important',
    marginLeft: '1.2em !important',
  },
  importButton: {
    display: 'block !important',
    margin: '1em !important',
    marginRight: '.5em !important',
    backgroundColor: '#F9A825',
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

  GameHints: true,

  NoArcane: false,
  MagicUpgrade: 'Vanilla',
  BookAmount: 1,

  BossHunt: false,
  MagicMaster: false,
  AllChestOpened: false,

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
  ItemWeightArcane: 2,

  SelectedSkin: 'Witch',
  RandomizeSkin: 'Never',
  HideBag: false,
  HideStaff: false,
  HideHat: false,
  BonusInitialized: false
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

  async function clipboardExport(data: any) {
    console.log('Data exported to clipboard:');
    console.log(data);

    await fetch("/clipboard-export", {
        method: 'POST',
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(data, null, 4)
    })
  }

  async function clipboardImport(oldData: any) {
    let response = await fetch("/clipboard-import", {
      method: 'GET',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
    }})

    let newData = await response.json();

    newData.SelectedSkin = oldData.SelectedSkin;
    newData.RandomizeSkin = oldData.RandomizeSkin;
    newData.HideBag = oldData.HideBag;
    newData.HideStaff = oldData.HideStaff;
    newData.HideHat = oldData.HideHat;
    newData.BonusInitialized = oldData.BonusInitialized;

    console.log('Data imported from clipboard:');
    console.log(newData);

    setData(newData);
  }

  useEffect(() => {
    async function getBonusSettings() {
      let response = await fetch("/bonus", {
        method: 'GET',
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json'
      }})

      let bonusSettings = await response.json();

      console.log('Initialization data:');
      console.log(bonusSettings);

      initialData.SelectedSkin = bonusSettings.SelectedSkin;
      initialData.RandomizeSkin = bonusSettings.RandomizeSkin;
      initialData.HideBag = bonusSettings.HideBag;
      initialData.HideStaff = bonusSettings.HideStaff;
      initialData.HideHat = bonusSettings.HideHat;
      initialData.BonusInitialized = true;

      setData(initialData);
    }

    getBonusSettings();
  }, []);

  return (
    <Grid
      container
      justifyContent={'center'}
      spacing={1}
      style={classes.container}>
      <Grid item xs={12} md={10} lg={8} xl={6} >
        <Typography variant={'h4'}>Settings</Typography>

        <Button
          style={classes.exportButton}
          onClick={async () => await clipboardExport(data)}
          color="primary"
          variant="contained"
          data-testid="clipboard-export"
          id='clipboard-export'>
          Export to Clipboard
        </Button>
        <Button
          style={classes.importButton}
          onClick={async () => await clipboardImport(data)}
          color="primary"
          variant="contained"
          data-testid="clipboard-import"
          id='clipboard-import'>
          Import from Clipboard
        </Button>

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
