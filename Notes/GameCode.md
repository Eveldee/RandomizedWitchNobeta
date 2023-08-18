# Research

## Chest `TreasureBox`

### Inspect

- Open status: `hasOpened`
- Item type: `ItemType`
- Open chest _(also respawn item)_: `SetOpen()`

### Change content

Patch `TreasureBox.Init` with a postfix and change `ItemType`. It is possible to filter chests using their name _(`TreasureBox02_Room03`)_.

## Change Scene load destination (useful for cutscene TP)

- Patch `Game.SwitchScene(SceneSwitchData sceneData, float fadeInDuration)`
- Filter on `sceneData.nextSceneName` _(can use GameStage enum with ToString apparently)_ and `sceneData.savePointNumber` _(`-1` is used for stage start)_
- Call `Game.SwitchScene` with wanted destination and cancel original method invocation _(`return false`)_
- Otherwise keep original invocation (`return true`)

## Change door destination

- Change `TransferLevelNumber` to desired destination level
- Change `TransferSavePointNumber` to this level start point _(different for each level, for example it's `2` for Lava Ruins)_

> A door is a `SavePoint` just like any other save point but it has in addition `PassiveEvent.EventType == Exit`

## Extra door flags

- Stage 01: `Stage 01 Open Door 01`, `Stage 01 Room 08 Door`
- Stage 02: `Stage 02 L 03 Back Door`