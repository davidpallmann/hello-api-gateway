# Hello, API Gateway!

This is the code project for the [Hello, API Gateway!](https://davidpallmann.hashnode.dev/hello-api-gateway) blog post. 

This episode: Amazon API Gateway. In this Hello, Cloud blog series, we're covering the basics of AWS cloud services for newcomers who are .NET developers. If you love C# but are new to AWS, or to this particular service, this should give you a jumpstart.

In this post we'll introduce API Gateway and use it to create a "Hello, Cloud" .NET API to perform [project-purpose]. We'll do this step-by-step, making no assumptions other than familiarity with C# and Visual Studio. We're using Visual Studio 2022 and .NET 6.

## Our Hello, API Gateway Project

We will create 2 AWS Lambda functions, one for storing menu data and one for retrieving it. Menu data will be stored in and retrieved from S3. Then, we'll create a REST API for storing and retrieving restaurant menus that integrates to the AWS Lambda functions. 

See the blog post for the tutorial to create this project and run it on AWS.

