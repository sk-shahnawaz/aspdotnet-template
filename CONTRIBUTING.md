# Contributing

## Git Workflow:
#### Clone the repository (For first time only)
```
cd <LOCATION_IN_FILE_SYSTEM_WHERE_PROJECT_WILL_BE_HOSTED>
git clone <GIT_HUB_REPO_URL>
```

There is one permanent branch in this repository: **'master'**.
Direct commits to *'master'* is *prohibited*. Instead a developer should start his/her work by first taking latest from 'master' branch.

#### Take latest from 'master'
```
cd <LOCATION_IN_FILE_SYSTEM_WHERE_PROJECT_IS_HOSTED>
git switch master
git fetch
git pull
```

#### Create a new feature branch from 'master' by using the following command
```
git checkout -b <TYPE_OF_WORK>/<NAME_OF_BRANCH>
```

Example:

|Type of Work|Meaning|Example|
|------------|-------|-------|
|feat| To add a new feature to the project|**feat/add-device-apis**|
|fix| To fix an issue|**fix/#100-object-ref-error** (Here, '#100' is ticket/issue ID)|

Please see [https://www.conventionalcommits.org/en/v1.0.0](GitHub Conventional Commits) and follow for smooth CI/CD flow.

#### Stage your changes
```
git add . 
```
or 
stage selective files:
```
git add file1.cs file2.cs
```

#### Commit the changes to your local repository in the created branch
```
git commit -m "<TYPE_OF_WORK>: <SHORT_BUT_MEANINGFUL_COMMIT_MESSAGE>"
```

Example:
- *git commit -m "feat: Filter functionality added to API."*
- *git commit -m "fix: #100 fixed issue related to object reference error"*

#### Push changes into GitHub
```
git push origin <LOCAL_BRANCH_NAME>
```

Example:
- *git push origin feat/add-odata*

### GitHub Workflow
- Upon pushing new branch to GitHub, PR needs to be created, please assign yourself and your supervisor as the reviewer.
- Address PR comments (if any, by changing implementation, committing into your local repository branch and pushing to GitHub again as a commit into the same GitHub branch).
- When PR is approved, need to merge it to 'master'. This can be done in GitHub PR's UI itself.
- Upon merging to 'master', GitHub Actions defined in 'master' branch gets triggered.

#### PR Title
PR title has to follow defined convention in order for the CI/CD to work. This is defined as:
```
<TYPE_OF_WORK>: MEANINGFUL_PR_TITLE
```

Example:
- *feat: Added filteration feature in Web API.*
- *fix: Standardization of variable names.*

#### Major Version Release
Please add the word "**#major**" in the commit message in order to bump up release version to the next number.
See [https://github.com/anothrNick/github-tag-action](Bump up Release Version by GitHub action)

Example:
- *feat: #major change for the data format change and the API end point*