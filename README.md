# Age of Empires 2 Definitive Edition Custom Font Generator

*[简体中文](README-CN.md)*

This project provides tools to genearte game font atlas files for Age of Empires 2 Definitive Edition, in order to solve the charset imcompolete problem and imrpvoe display effect.

**Important: Please read the frequently QnA before asking for help.**

## How to Use

To use this tools to replace the in-game font, please follow these steps:

1. Open this tool, set one or more typeface and charset mappings.
2. Click "Start Generation" Button to generate game font files.
3. Copy all generated files and replace the original files in "resources/_common/fonts" folder. Note it may generate more files than the game original contains. 
4. Start game and you will see the effect in the game lobby, tech-tree and game-playing UI. Note the start screen is not affected.
5. If you'd like to adjust the display effect or change fonts, repeat the above steps again.

## Frequently QnA

#### The generation is aborted with error displayed

Please read the error message and operate follow the instuction. If you still have problems, you may submit new issues in this project.

#### The applicaion carshed

Please submit a new issue with detailed reproduce step to help the author improve this tool.

#### The glyphs are too large/small in the game

This is caused by your selected typeface not correctly response with glyphing size requests. Please enter the detailed mapping setting panel, and try to adjust the "glyph scaling ratio" value to resize the generated glyphs. Note change the global glyph size will not work for this problem.

#### The glyphs are not vertical algined with the game UI

This is caused by your selected typeface has an incorrect baseline value. Please enter the detailed mapping setting panel, and try to adjust the "baseline offset ratio" to adjust the vertical location of glyphs.

#### Some glyphs lookes chaotic

This is caused by the glyph size too large (may not obvious when displaying) and the Video RAM is damaged when reading glyphs. Please refer to the "The glyphs are too large/small in the game" section to solve this problem.
