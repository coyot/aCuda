#include <iostream>

/*extern "C" __global__ void add(int a, int b, int *c) {
	*c = a + b;
}*/

extern "C" __global__ void count_frequency(int * input, int * output, unsigned width, unsigned height) 
{
	int baseX = blockIdx.x * blockDim.x + threadIdx.x;
	int totalThreads = blockDim.x * gridDim.x;

	for(int elementIndex = baseX; elementIndex < width; elementIndex += totalThreads) {

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
/*
extern "C" __global__ void add_vector(int *a, int *b, int *c) {

	int tid = blockIdx.x * blockDim.x + threadIdx.x;
	int totalThreads = blockDim.x * gridDim.x;

	while(tid < 10) {
		c[tid] = a[tid] + b[tid];
		tid += totalThreads;
	}
}*/