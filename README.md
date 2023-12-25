# Age of Empires 2 Definitive Edition Custom Font Generator

*[简体中文](README-CN.md)*

This project provides tools to genearte game font atlas files for the *Age of Empires 2 Definitive Edition*, in order to solve the charset imcomplete problem as well as improve in-game display effect.

**Important: Please read the frequently Q&A before asking for help.**

## How to Use

To use this tools to replace the in-game font, please follow the steps below:

1. Open this tool, set one or more typeface and charset mappings.
2. Click "Start Generation" Button to generate game font files.
3. Copy generated files (all files with extension ".dds" and ".box", other files can be ignored) and replace the original files in "resources/_common/fonts" folder. Note it may generate more files than the game original contains. 
4. Start game and you will see the effect in the game lobby, tech-tree and game-playing UI. Note the start screen is not affected.
5. If you'd like to adjust the display effect or change fonts, repeat the above steps again.

## Frequently Q&A

#### Generation is aborted with an error displayed

Please read the error message and operate following the instruction. If you still have any problem, you may submit a new issue in this project.

#### The applicaion crashs

Please submit a new issue with detailed reproducing steps to help the author improve this tool.

#### The glyphs are too large/small in the game

This is caused by your selected typeface does not correctly response with glyph size requests. Please open the detailed mapping setting panel and try to adjust the "glyph scaling ratio" value to resize the generated glyphs. Note that changing the global glyph size does not work for this problem.

#### The glyphs are not vertical algined with the game UI

This is caused by your selected typeface has an incorrect baseline value. Please open the detailed mapping setting panel and try to adjust the "baseline offset ratio" to adjust the vertical location of glyphs.

#### Some glyphs look chaotic

This is caused by the glyph size is too large (although may not obvious when displaying) and the Video RAM is damaged when loading glyphs. Please refer to the "The glyphs are too large/small in the game" section to solve this problem.

#### Some glyphs become question marks

This should be caused if any glyph is missing. Your generated font file does not contain the character should be displayed and thus the game replace it with a question mark. Please consider to change the font source or add a secondary font source to supply missing chareacters.
