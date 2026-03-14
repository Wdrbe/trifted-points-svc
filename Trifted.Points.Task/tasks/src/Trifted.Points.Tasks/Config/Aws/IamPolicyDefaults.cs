// Copyright (c) Kanject 2023
// Author:  Omogbolahan Akinsanya

namespace Trifted.Points.Tasks.Config.Aws;

public static class IamPolicyDefaults
{
    public const string FunctionPolicy = "AWSLambda_FullAccess,arn:aws:iam::147997163791:policy/get_trifted_params,arn:aws:iam::aws:policy/service-role/AWSLambdaSQSQueueExecutionRole";
}