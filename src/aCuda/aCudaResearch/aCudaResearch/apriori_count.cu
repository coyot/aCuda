#include <iostream>

extern "C" __global__ void count_frequency(int * input, int * output, unsigned width, unsigned height) 
{
	int baseX = blockIdx.x * blockDim.x + threadIdx.x;
	int totalThreads = blockDim.x * gridDim.x;

	for(int elementIndex = baseX; elementIndex < width; elementIndex += totalThreads) 
	{
		int i = elementIndex;
		int sum = 0;

		while(i < width * height) 
		{
			sum += input[i];
			i += width;
		}

		output[elementIndex] = sum;
	}
}

extern "C" __global__ void count_frequency_matrix(int * input, int * inputSets, int * output, 
												 unsigned width, unsigned height, unsigned setWidth, unsigned sets) 
{
	int baseX = blockIdx.x * blockDim.x + threadIdx.x;
	int totalThreads = blockDim.x * gridDim.x;

	for(int c = 0; c < sets; c++) 
	{
		int occuredSum = 0;

		for(int tid = baseX; tid < height; tid += totalThreads) 
		{
			int sum = 0;
			for(int i = 0; i < setWidth; i++)
			{
				sum += input[tid * width + inputSets[i]];
			}
			// we found in the transaction all elements from the checked set
			if(sum == setWidth) 
			{
				occuredSum++;	
			}
		}

		output[baseX + c * totalThreads] = occuredSum;
	}
}

// count sum for each column (which is support for candidate!)
extern "C" __global__ void count_frequency_table(int * input, int * output, 
												 unsigned width, unsigned height) 
{
	int baseX = blockIdx.x * blockDim.x + threadIdx.x;
	int totalThreads = blockDim.x * gridDim.x;

	for(int id = baseX; id < width; id += totalThreads) 
	{
		int sum = 0;

		while(id < width * height) 
		{
			sum += input[id];
			id += width;
		}

		output[id] = input[id];
	}
}