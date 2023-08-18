# Logic

# Areas

- [Act02\_01](#act02_01)
  - [Chests](#chests)
  - [Special Loot](#special-loot)
  - [Save Points](#save-points)
  - [Exits](#exits)
- [Act03\_01](#act03_01)
  - [Chests](#chests-1)
  - [Special Loot](#special-loot-1)
  - [Save Points](#save-points-1)
  - [Exits](#exits-1)
- [Act04\_01](#act04_01)
  - [Chests](#chests-2)
  - [Special Loot](#special-loot-2)
  - [Save Points](#save-points-2)
  - [Exits](#exits-2)
- [Act05\_02](#act05_02)
  - [Chests](#chests-3)
  - [Special Loot](#special-loot-3)
  - [Save Points](#save-points-3)
  - [Exits](#exits-3)
- [Act06\_03](#act06_03)
  - [Chests](#chests-4)
  - [Special Loot](#special-loot-4)
  - [Save Points](#save-points-4)
  - [Exits](#exits-4)
- [Act07](#act07)
  - [Chests](#chests-5)
  - [Special Loot](#special-loot-5)
  - [Save Points](#save-points-5)
  - [Exits](#exits-5)


## Act02_01

### Chests

| Name                 | Content        | Required                                          |
| -------------------- | -------------- | ------------------------------------------------- |
| TreasureBox_Room03   | HPCure         |                                                   |
| TreasureBox02_Room03 | BagMaxAdd      |                                                   |
| TreasureBox_Room05   | Defense        | Arcane 1 OR Thunder 1                             |
| TreasureBox07        | Absorb         | (Arcane 1 OR Thunder 1) AND (Fire 1 OR Thunder 1) |
| TreasureBox07To08    | SkyJump        | (Arcane 1 OR Thunder 1) AND (Fire 1 OR Thunder 1) |
| TreasureBox08        | MagicIce       | (Arcane 1 OR Thunder 1) AND (Fire 1 OR Thunder 1) |
| TreasureBox09        | MagicFire      | (Arcane 1 OR Thunder 1) AND (Fire 1 OR Thunder 1) |
| TreasureBox10        | MagicLightning | (Arcane 1 OR Thunder 1) AND (Fire 1 OR Thunder 1) |

### Special Loot

### Save Points

| Location            | Teleport Index | Real Index | Required                                          |
| ------------------- | -------------- | ---------- | ------------------------------------------------- |
| Load                | N/A            | -1         |                                                   |
| First Hall          | 0              | 0          |                                                   |
| After first barrier | 1              | 3          |                                                   |
| Armor Hall          | 2              | 1          | Arcane 1 OR Thunder 1                             |
| Secret Passage      | 3              | 4          | (Arcane 1 OR Thunder 1) AND (Fire 1 OR Thunder 1) |

### Exits

| Location            | Next Scene            | Next SavePoint | Required                                          |
| ------------------- | --------------------- | -------------- | ------------------------------------------------- |
| Kill Armor          | [Act03_01](#act03_01) | -1             | Arcane 1 OR Thunder 1                             |
| Door before Armor   | [Act03_01](#act03_01) | 2              | Arcane 1 OR Thunder 1                             |
| Secret Passage Door | [Act05_02](#act05_02) | 5              | (Arcane 1 OR Thunder 1) AND (Fire 1 OR Thunder 1) |

## Act03_01

### Chests

| Name                  | Content    | Required                               |
| --------------------- | ---------- | -------------------------------------- |
| TreasureBox_Room01    | SkyJump    |                                        |
| TreasureBox_Room03    | HPCure     |                                        |
| TreasureBox_Room04    | Defense    |                                        |
| TreasureBox_Room05_01 | MagicNull  |                                        |
| TreasureBox_Room05_02 | MagicIce   |                                        |
| TreasureBox_Room08    | Mysterious | **Ice 1** _(using thunder is illegal)_ |

### Special Loot

| Action      | Content | Required |
| ----------- | ------- | -------- |
| Talk to cat | Absorb  |          |

### Save Points

| Location     | Teleport Index | Real Index | Required                               |
| ------------ | -------------- | ---------- | -------------------------------------- |
| Load         | N/A            | -1         |                                        |
| Cat Dialog   | 0              | 0          |                                        |
| Early return | N/A            | 6          |                                        |
| Cave Exit    | 1              | 1          | **Ice 1** _(using thunder is illegal)_ |
| After Tania  | N/A            | 5          | **Ice 1** _(using thunder is illegal)_ |

### Exits

| Location           | Next Scene            | Next SavePoint | Required                               |
| ------------------ | --------------------- | -------------- | -------------------------------------- |
| Door to Armor      | [Act02_01](#act02_01) | 2              | **Ice 1** _(using thunder is illegal)_ |
| Door to Lava Ruins | [Act04_01](#act04_01) | 3              | **Ice 1** _(using thunder is illegal)_ |
| Door after Tania   | [Act04_01](#act04_01) | 2              | **Ice 1** _(using thunder is illegal)_ |

## Act04_01

### Chests

| Name                     | Content      | Required            |
| ------------------------ | ------------ | ------------------- |
| Room02_TreasureBox01     | BagMaxAdd    |                     |
| Room02_TreasureBox02     | Defense      |                     |
| Room03_TreasureBox01     | HPCureMiddle |                     |
| Room03_TreasureBox02     | Absorb       |                     |
| Room02To04_TreasureBox02 | SkyJump      |                     |
| Room05To06_TreasureBox   | HolyM        |                     |
| Room06_TreasureBox       | MagicFire    |                     |
| Room07_TreasureBox       | Defense      | Fire 1 OR Thunder 1 |
| Room08_TreasureBox       | MagicNull    | Fire 1 OR Thunder 1 |
| Room01_TreasureBox       | MagicIce     | Fire 1 OR Thunder 1 |

### Special Loot

### Save Points

| Location    | Teleport Index | Real Index | Required            |
| ----------- | -------------- | ---------- | ------------------- |
| Load        | N/A            | 2          |                     |
| Main Hall   | 0              | 0          |                     |
| Before Bear | 1              | 1          | Fire 1 OR Thunder 1 |

### Exits

| Location                   | Next Scene            | Next SavePoint | Required            |
| -------------------------- | --------------------- | -------------- | ------------------- |
| Door to Tania **(LOCKED)** | [Act03_01](#act03_01) | 3              |                     |
| Door to Underground Hall   | [Act03_01](#act03_01) | 4              | Fire 1 OR Thunder 1 |
| Door to Dark Tunnel        | [Act05_02](#act05_02) | 3              | Fire 1 OR Thunder 1 |

## Act05_02

### Chests

| Name                     | Content        | Required  |
| ------------------------ | -------------- | --------- |
| TreasureBox02_Room02_03  | Absorb         |           |
| TreasureBox02_Room03_02  | MysteriousB    |           |
| TreasureBox02_Room03_01  | SkyJump        |           |
| TreasureBox02_Room04     | MagicFire      |           |
| TreasureBox02_Room05     | MagicLightning |           |
| TreasureBox02_Room06To07 | DefenseB       | Thunder 1 |
| TreasureBox02_Room07     | HolyB          | Thunder 1 |
| TreasureBox02_Room08     | MagicNull      | Thunder 1 |
| TreasureBox02_Room09To10 | BagMaxAdd      | Thunder 1 |

### Special Loot

### Save Points

| Location     | Teleport Index | Real Index | Required  |
| ------------ | -------------- | ---------- | --------- |
| Load         | N/A            | 3          |           |
| Hat          | 0              | 0          |           |
| Thunder      | 1              | 1          |           |
| Castle Entry | 2              | 6          | Thunder 1 |
| Vanessa      | 3              | 2          | Thunder 1 |

### Exits

| Location                        | Next Scene            | Next SavePoint | Required  |
| ------------------------------- | --------------------- | -------------- | --------- |
| Door to Lava Ruins **(LOCKED)** | [Act04_01](#act04_01) | 5              |           |
| Door at Vanessa                 | [Act02_01](#act02_01) | 5              | Thunder 1 |
| Kill Vanessa                    | [Act06_03](#act06_03) | -1             | Thunder 1 |

## Act06_03

### Chests

| Name                | Content        | Required                         |
| ------------------- | -------------- | -------------------------------- |
| TreasureBox02_R02   | SkyJump        |                                  |
| TreasureBox02_R03   | DefenseB       |                                  |
| TreasureBox02_R0401 | MagicIce       |                                  |
| TreasureBox02_R0402 | MagicLightning | Arcane 1                         |
| TreasureBox02_R06   | MagicFire      | Arcane 1                         |
| TreasureBox02_R07   | Absorb         | Arcane 1                         |
| TreasureBox02_R08   | HolyB          | Arcane 1 AND Ice 1 AND Thunder 1 |

### Special Loot

| Action         | Content | Required                         |
| -------------- | ------- | -------------------------------- |
| Kill Vanessa 2 | Thunder | Arcane 1 AND Ice 1 AND Thunder 1 |

### Save Points

| Location  | Teleport Index | Real Index | Required                         |
| --------- | -------------- | ---------- | -------------------------------- |
| Load      | N/A            | -1         |                                  |
| Awake     | 0              | 0          |                                  |
| Pre Seal  | 1              | 1          |                                  |
| Post Seal | 2              | 2          | Arcane 1                         |
| Vanessa 2 | 3              | 3          | Arcane 1 AND Ice 1 AND Thunder 1 |

### Exits

| Location       | Next Scene      | Next SavePoint | Required                         |
| -------------- | --------------- | -------------- | -------------------------------- |
| Kill Vanessa 2 | [Act07](#act07) | -1             | Arcane 1 AND Ice 1 AND Thunder 1 |

## Act07

### Chests

| Name                           | Content        | Required                        |
| ------------------------------ | -------------- | ------------------------------- |
| TreasureBox_Act02Room04        | BagMaxAdd      |                                 |
| TreasureBox_Act02Room05        | MagicNull      |                                 |
| Act04Room05To06_TreasureBox    | MagicIce       | Arcane 1 OR Fire 1 OR Thunder 1 |
| Act05_TreasureBox02_Room09To10 | MagicLightning | Arcane 1 OR Fire 1 OR Thunder 1 |
| Act03TreasureBox_Room05_02     | MagicFire      | Arcane 1 OR Fire 1 OR Thunder 1 |

### Special Loot

### Save Points

| Location       | Teleport Index | Real Index | Required                        |
| -------------- | -------------- | ---------- | ------------------------------- |
| Load           | N/A            | -1         |                                 |
| Path to Prison | 0              | 0          |                                 |
| Trials         | 1              | 1          | Arcane 1 OR Fire 1 OR Thunder 1 |
| Nonota         | 2              | 2          | Ice 1 AND (Fire 1 OR Thunder 1) |

### Exits

| Location    | Next Area | Required                        |
| ----------- | --------- | ------------------------------- |
| Kill Nonota | **END**   | Ice 1 AND (Fire 1 OR Thunder 1) |