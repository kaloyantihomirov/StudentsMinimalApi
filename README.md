# StudentsMinimalApi
I created an example minimal API that exposes endpoints that you can use to access or change data related to an example Student class using the HTTP protocol.

I also tried to predict common problems that may occur when a client calls my API and return the most appropriate status codes for the particular case.

Problem Details is used for providing machine-readable errors for HTTP APIs.

I used endpoint filters for common validation code. I also used a factory pattern for them to be more reusable. A filter factory is a generalised way to add endpoint filters.
I used a filter factory for a general way to validate if the id of a student is not empty and starts with 's'.
