# CodeJanitor

CodeJanitor is an auto-code fixer with Claude Code that integrates with your CI platform (like GitLab, GitHub) to automatically fix code quality issues. Currently, only GitLab is supported.

## Known issues:
- [ ] Add key revalidation - the key can go rotten, the user can revoke the key, as a result - we have a repository with which we cannot do anything.
- [ ] Saving to some intermediate repository coming through webhook issues - for reprocessing in case of errors.
- [ ] Add authentication.
- [ ] Add e2e tests.