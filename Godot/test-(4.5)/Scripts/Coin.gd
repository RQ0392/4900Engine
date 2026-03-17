extends Area3D

func _ready() -> void:
	# 连接信号：当有物体进入触发区时，运行下面的函数
	body_entered.connect(_on_body_entered)

func _on_body_entered(body: Node) -> void:
	# 检查撞进来的是不是玩家（确保你的 Player 在 "Player" 组里）
	if not body.is_in_group("Player"):
		return

	# 打印作业要求的精确字符串
	print("Coin collected!")
	
	# 额外挑战：捡到后让金币消失
	queue_free()
