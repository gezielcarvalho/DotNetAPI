<p align="center">
<a href="http://sabresoftware.com.br/" target="blank"><img src="https://user-images.githubusercontent.com/16593463/209469380-8124ba8d-79bf-419a-a157-79d2f6678621.png" width="200" alt="Nest Logo" /></a>
</p>

# DotNet API

## Description

This is a REST API developed with DotNet 6 Core.
It is a simple API that allows you to register and login a user. 
The authentication is done with JWT over a Bearer token. 
The API is documented with Swagger and the documentation can be found at the endpoint `/api/documentation`.

## Development process

The development process was done using a simplified version of [Gitflow Workflow](https://www.atlassian.com/git/tutorials/comparing-workflows/gitflow-workflow). The main branch is `main`. The features are developed in branches named `feat/<feature-name>` and the hotfixes are developed in branches named `hotfix/<hotfix-name>`. The features and hotfixes are merged into `main` when a release is made, since this is just a demo project. In a real project, the features and hotfixes would be merged into `develop` and the `main` branch would be used only for releases.

### Before starting a new feature

```bash
$ git checkout main
$ git pull
$ git checkout -b feat/<feature-name>
```

# References

- [Gitflow Workflow](https://www.atlassian.com/git/tutorials/comparing-workflows/gitflow-workflow)
- [MICROSOFT ARCHITECT](https://www.infoworld.com/article/3669188/how-to-implement-jwt-authentication-in-aspnet-core-6.html)

