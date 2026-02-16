extends Node

# Use 'Node' type here instead of 'Receiver' to avoid type-recognition errors
@export var receiver: Node 

func _ready():
	# Primary log
	print("Hello Friend")
	
	# Check if the reference is assigned before calling
	if receiver:
		# Note: GDScript is dynamic, so it will find OnCalled() if it exists on the node
		receiver.OnCalled()
	else:
		# This will print to the Output if you forget to drag the node
		print("Error: Please drag the Receiver node into the Inspector slot!")
