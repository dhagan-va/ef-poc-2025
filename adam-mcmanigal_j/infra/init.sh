#!/bin/bash
set -e

BUCKET=edi-bucket
QUEUE=edi-queue
DLQ=edi-queue-dlq
ENDPOINT=http://moto:5000
REGION=us-east-1
ACCOUNT=123456789012

aws --endpoint-url=$ENDPOINT s3 mb s3://$BUCKET

aws --endpoint-url=$ENDPOINT sqs create-queue --queue-name $DLQ

DLQ_ARN=arn:aws:sqs:$REGION:$ACCOUNT:$DLQ

aws --endpoint-url=$ENDPOINT sqs create-queue \
  --queue-name $QUEUE \
  --attributes "{\"RedrivePolicy\":\"{\\\"deadLetterTargetArn\\\":\\\"$DLQ_ARN\\\",\\\"maxReceiveCount\\\":\\\"3\\\"}\"}"

QUEUE_ARN=arn:aws:sqs:$REGION:$ACCOUNT:$QUEUE

aws --endpoint-url=$ENDPOINT s3api put-bucket-notification-configuration \
  --bucket $BUCKET \
  --notification-configuration "{\"QueueConfigurations\":[{\"QueueArn\":\"$QUEUE_ARN\",\"Events\":[\"s3:ObjectCreated:*\"]}]}"

echo "Moto init complete: s3://$BUCKET -> sqs://$QUEUE (dlq: $DLQ)"
