### X-Alltag-Projekt

[Beschreibung]

**Downloading Repository**
```Shell
~ $ cd parent_dir                                       
parent_dir $ git clone [ssh|https]
parent_dir $ cd x-alltag-projekt
```

or

```Shell
~$ cd existing_repo
existing_dir $ git remote add origin https://gitlab.informatik.uni-bremen.de/pablo2/xalltag.git
existing_dir $ git branch -M main
existing_dir $ git pull
```

**Use of LFS (Large file system) to commit / push large files**

Installation Guide (german):
https://docs.github.com/de/repositories/working-with-files/managing-large-files/installing-git-large-file-storage

Some usefull commands for LFS:
For all of these commands you need to go to the project diretory via the cmd-line

Show all files-extensions that are currently covered by .gitAttributes:
`git lfs track`

Add a new file-extension to the LFS-system:
`git lfs track "*.something"`

Show all files that are currently tracked by lfs (those need to be already commited):
`git lfs ls-files --all`

For everything else see the documenation or ask google e.g.

**Roadmap**

**License**

**Project status**
Early development.

**Badges**
On some READMEs, you may see small images that convey metadata, such as whether or not all the tests are passing for the project. You can use Shields to add some to your README. Many services also have instructions for adding a badge.
